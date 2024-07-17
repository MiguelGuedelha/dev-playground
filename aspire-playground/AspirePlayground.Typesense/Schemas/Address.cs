using System.Text.Json.Serialization;

namespace AspirePlayground.Typesense.Schemas;

public class Address
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("house_number")]
    public int HouseNumber { get; set; }
    [JsonPropertyName("access_address")]
    public required string AccessAddress { get; set; }
    [JsonPropertyName("metadata_notes")]
    public required string MetadataNotes { get; set; }
}