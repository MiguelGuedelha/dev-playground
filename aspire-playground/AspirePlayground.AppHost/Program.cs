var builder = DistributedApplication.CreateBuilder(args);

const string baseVolumePath = "local/volumes/";

// Containers
var redis = builder
    .AddRedis("Cache")
    .WithRedisCommander();

var redisEndpoint = redis.GetEndpoint("http");

var typesense = builder
    .AddContainer("search", "typesense/typesense", "26.0")
    .WithHttpEndpoint(port: 8108, targetPort: 8108)
    .WithBindMount(baseVolumePath + "AspirePlayground-typesense-data", "/data")
    .WithArgs("--data-dir=/data", "--api-key=xyz", " --enable-cors");

var typesenseEndpoint = typesense.GetEndpoint("http");

// Projects
var apiService = builder.AddProject<Projects.AspirePlayground_ApiService>("apiservice")
        .WithReference(redis)
        .WithReference(redisEndpoint)
        .WithEnvironment("ConnectionStrings__Search", typesenseEndpoint);

builder.AddProject<Projects.AspirePlayground_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
