var builder = DistributedApplication.CreateBuilder(args);

const string baseVolumePath = "local/volumes/";

// Containers
var redis = builder
    .AddRedis("cache")
    .WithRedisCommander();

var typesense = builder
    .AddContainer("search", "typesense/typesense", "26.0")
    .WithHttpEndpoint(port: 8108, targetPort: 8108, name: "search-endpoint")
    .WithBindMount(baseVolumePath + "AspirePlayground-typesense-data", "/data")
    .WithArgs("--data-dir=/data", "--api-key=xyz", " --enable-cors");

var typesenseEndpoint = typesense.GetEndpoint("search-endpoint");

// Projects
var apiService = builder.AddProject<Projects.AspirePlayground_ApiService>("apiservice");

builder.AddProject<Projects.AspirePlayground_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.AspirePlayground_FusionCache>("fusion-cache")
    .WithReference(apiService)
    .WithReference(redis);

builder.AddProject<Projects.AspirePlayground_Typesense>("typesense-search")
    .WithReference(typesenseEndpoint);

builder.Build().Run();
