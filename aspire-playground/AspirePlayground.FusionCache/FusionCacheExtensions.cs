using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.NewtonsoftJson;

namespace AspirePlayground.FusionCache;

public static class FusionCacheExtensions
{
    public static void AddFusionCacheProjectServices(this IServiceCollection services)
    {
        services.AddFusionCache()
            .WithOptions(o =>
            {
                //Backs off the Distributed Cache if having issues
                o.DistributedCacheCircuitBreakerDuration = TimeSpan.FromSeconds(2);

                //Log Levels (examples)
                o.DistributedCacheErrorsLogLevel = LogLevel.Error;
                o.FactoryErrorsLogLevel = LogLevel.Error;
            })
            .WithDefaultEntryOptions(o =>
            {
                //General
                o.Duration = TimeSpan.FromSeconds(15);

                //Failsafe
                o.IsFailSafeEnabled = true;
                o.FailSafeMaxDuration = TimeSpan.FromMinutes(1);
                o.FailSafeThrottleDuration = TimeSpan.FromSeconds(5);

                //Factory Timeouts
                o.FactorySoftTimeout = TimeSpan.FromSeconds(1);
                o.FactoryHardTimeout = TimeSpan.FromSeconds(2);

                //Distributed Cache Options
                o.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1);
                o.DistributedCacheHardTimeout = TimeSpan.FromSeconds(2);
                o.AllowBackgroundDistributedCacheOperations = true;

                //Jitter
                o.JitterMaxDuration = TimeSpan.FromSeconds(2);
            })
            .WithSerializer(
                new FusionCacheNewtonsoftJsonSerializer()
            )
            .WithRegisteredDistributedCache()
            .WithBackplane(sp =>
            {
                return new RedisBackplane(new RedisBackplaneOptions
                {
                    ConnectionMultiplexerFactory = () =>
                        Task.FromResult(sp.GetRequiredService<IConnectionMultiplexer>())
                });
            });
    }

    public static void AddFusionCacheOpenTelemetry(this IServiceCollection builder)
    {
        builder.AddOpenTelemetry()
            .WithTracing(b =>
            {
                b.AddFusionCacheInstrumentation(o =>
                {
                    o.IncludeMemoryLevel = true;
                });
            })
            .WithMetrics(b =>
            {
                b.AddFusionCacheInstrumentation(o =>
                {
                    o.IncludeMemoryLevel = true;
                    o.IncludeDistributedLevel = true;
                    o.IncludeBackplane = true;
                });
            });
    }
}