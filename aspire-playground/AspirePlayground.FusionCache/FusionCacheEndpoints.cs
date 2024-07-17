using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace AspirePlayground.FusionCache;

public static class FusionCacheEndpoints
{
    public static void MapFusionCacheEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("caching");

        group.MapGet("{seed:int}", GenerateRandomCacheData);
    }

    private static async Task<List<int>> GenerateRandomCacheData(int seed, IFusionCache cache)
    {
        var random = new Random(seed);

        return await cache.GetOrSetAsync($"{seed}", async t =>
        {
            var failureRandom = Random.Shared.Next(1, 11);
            var factoryDelayRandom = Random.Shared.Next(500, 2001);
            
            Console.WriteLine($"{nameof(GenerateRandomCacheData)} | {new { factoryDelayRandom, failureRandom }}");
            
            if (failureRandom > 5)
            {
                // 50% chance of factory failing
                throw new InvalidOperationException();
            }
            
            
            await Task.Delay(factoryDelayRandom, t);
            
            return Enumerable.Range(0, seed).Select(_ => random.Next()).ToList();
        });
    }
}