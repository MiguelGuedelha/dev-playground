var builder = DistributedApplication.CreateBuilder(args);

var redisContainer = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspirePlayground_ApiService>("apiservice");

builder.AddProject<Projects.AspirePlayground_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.AspirePlayground_FusionCache>("fusion-cache-tester")
    .WithReference(apiService)
    .WithReference(redisContainer);

builder.Build().Run();
