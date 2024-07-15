using System.Text.Json;
using Typesense;
using Typesense.Setup;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddTypesenseClient(config =>
{
    config.ApiKey = "xyz";
    config.Nodes = new[]
    {
        new Node("search-endpoint", "80")
    };
}, enableHttpCompression: false);

var host = builder.Build();

// Test code
var typesenseClient = host.Services.GetRequiredService<ITypesenseClient>();

var schema = new Schema(
    "Addresses",
    new List<Field>
    {
        new("id", FieldType.Int32, false),
        new("houseNumber", FieldType.Int32, false),
        new("accessAddress", FieldType.String, false, true),
        new("metadataNotes", FieldType.String, false, true, false),
    },
    "houseNumber");

while (true)
{
    if (await typesenseClient.RetrieveCollection("Addresses") is { } collection)
    {
        Console.WriteLine(JsonSerializer.Serialize(collection, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }
    else
    {
        var createCollectionResult = await typesenseClient.CreateCollection(schema);

        Console.WriteLine(JsonSerializer.Serialize(createCollectionResult, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }

    await Task.Delay(5000);
}

//host.Run();