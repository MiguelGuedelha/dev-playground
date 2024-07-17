using Microsoft.Extensions.DependencyInjection;
using Typesense.Setup;

namespace AspirePlayground.Typesense;

public static class TypesenseExtensions
{
    public static void AddTypesenseProjectServices(this IServiceCollection services)
    {
        services.AddTypesenseClient(config =>
        {
            config.ApiKey = "xyz";
            config.Nodes = new[]
            {
                new Node("search-endpoint", "8108")
            };
        }, enableHttpCompression: false);

        services.AddScoped<ITypesenseService, TypesenseService>();
    }
}