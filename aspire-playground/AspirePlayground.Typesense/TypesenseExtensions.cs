using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Typesense.Setup;

namespace AspirePlayground.Typesense;

public static class TypesenseExtensions
{
    public static void AddTypesenseProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        var url = new Uri(configuration.GetConnectionString("Search")!);

        services.AddTypesenseClient(config =>
        {
            config.ApiKey = "xyz";
            config.Nodes = new[]
            {
                new Node(url.Host, url.Port.ToString())
            };
        }, false);

        services.AddScoped<ITypesenseService, TypesenseService>();
    }
}