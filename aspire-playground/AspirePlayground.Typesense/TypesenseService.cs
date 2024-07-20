using Typesense;

namespace AspirePlayground.Typesense;

public class TypesenseService : ITypesenseService
{
    private readonly ITypesenseClient _client;

    public TypesenseService(ITypesenseClient client)
    {
        //Assert
        ArgumentNullException.ThrowIfNull(client);

        //Assign
        _client = client;
    }

    public async Task<CollectionResponse> CreateSchema(string collectionName, List<Field> fields,
        string? sortingFieldName)
    {
        return sortingFieldName switch
        {
            not null => await _client.CreateCollection(new(collectionName, fields, sortingFieldName)),
            _ => await _client.CreateCollection(new(collectionName, fields))
        };
    }

    public async Task<UpdateCollectionResponse> UpdateSchema(string collectionName,
        List<UpdateSchemaField> fieldsToUpdate)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpsertDocument<T>(string collectionName, T document) where T : class
    {
        return await _client.UpsertDocument(collectionName, document);
    }

    public async Task<T> GetDocumentById<T>(string collectionName, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateDocumentById<T>(string collectionName, string id, T document)
    {
        throw new NotImplementedException();
    }

    public async Task<T> DeleteDocumentById<T>(string collectionName, string id, T document)
    {
        throw new NotImplementedException();
    }
}

public interface ITypesenseService
{
    Task<CollectionResponse> CreateSchema(string collectionName, List<Field> fields, string? sortingFieldName);
    Task<UpdateCollectionResponse> UpdateSchema(string collectionName, List<UpdateSchemaField> fieldsToUpdate);
    Task<T> UpsertDocument<T>(string collectionName, T document) where T : class;
    Task<T> GetDocumentById<T>(string collectionName, string id);
    Task<T> UpdateDocumentById<T>(string collectionName, string id, T document);
    Task<T> DeleteDocumentById<T>(string collectionName, string id, T document);
}