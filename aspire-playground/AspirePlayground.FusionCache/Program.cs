using AspirePlayground.FusionCache;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Refit;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.NewtonsoftJson;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisDistributedCache("cache");

builder.Services.AddFusionCache()
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
        o.Duration = TimeSpan.FromSeconds(3);
        
        //Failsafe
        o.IsFailSafeEnabled = true;
        o.FailSafeMaxDuration = TimeSpan.FromHours(1);
        o.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);
        
        //Factory Timeouts
        o.FactorySoftTimeout = TimeSpan.FromMilliseconds(500);
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
    .WithBackplane(
        new RedisBackplane(new RedisBackplaneOptions { Configuration = "redis" })
    );

builder.Services.AddOpenTelemetry()
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

builder.Services.AddRefitClient<IWeatherClient>();

var host = builder.Build();
host.Run();