using System.Text.Json.Serialization;

namespace AspirePlayground.Typesense.Schemas;

public class Address
{
    
    public string Id { get; set; }
    public int HouseNumber { get; set; }
    public string AccessAddress { get; set; } = string.Empty;
    public string MetadataNotes { get; set; } = string.Empty;
}