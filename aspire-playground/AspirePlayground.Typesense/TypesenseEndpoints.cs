using AspirePlayground.Typesense.Schemas;
using Microsoft.AspNetCore.Builder;
using Typesense;

namespace AspirePlayground.Typesense;

public static class TypesenseEndpoints
{
    public static void MapTypesenseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("typesense");

        //Collections
        var collectionGroup = group.MapGroup("collections");
        collectionGroup.MapPost("", CreateCollection);
        
        //Documents
        var documentsGroup = group.MapGroup("documents");
        documentsGroup.MapPost("addresses", UpsertAddress);
    }

    private static async Task<CollectionResponse> CreateCollection(CreateCollectionRequest request, ITypesenseService service)
    {
        return await service.CreateSchema(
            request.CollectionName, 
            request.Fields.Select(x => new Field(x.Name, x.Type, x.IsFacet, x.IsOptional, x.IsIndexed)).ToList(),
            request.SortingField);
    }
    
    private static async Task<Address> UpsertAddress(UpsertDocumentRequest<Address> request, ITypesenseService service)
    {
        return await service.UpsertDocument(request.CollectionName, request.Document);
    }
}

record CreateCollectionRequest(string CollectionName, List<CreateCollectionRequestFields> Fields, string? SortingField);

record CreateCollectionRequestFields(string Name, FieldType Type, bool IsFacet, bool IsOptional, bool IsIndexed);

record UpsertDocumentRequest<T>(string CollectionName, T Document);