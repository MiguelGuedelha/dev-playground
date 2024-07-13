var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspirePlayground_ApiService>("apiservice");

builder.AddProject<Projects.AspirePlayground_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
