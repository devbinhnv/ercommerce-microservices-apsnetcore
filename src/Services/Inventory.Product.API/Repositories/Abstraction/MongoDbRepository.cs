using Inventory.Product.API.Entities.Abstraction;
using Inventory.Product.API.Extensions;
using Inventory.Product.API.Extensions.Attributes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharpCompress.Common;
using System.Linq.Expressions;
using System.Reflection;
using ZstdSharp.Unsafe;

namespace Inventory.Product.API.Repositories.Abstraction;

public class MongoDbRepository<T> : IMongoDbRepositoryBase<T>
    where T : MongoEntity
{
    private IMongoDatabase Database { get; }

    public MongoDbRepository(IMongoClient client, IOptions<DatabaseSettings> option)
    {
        var settings = option.Value;
        Database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
        => Database.WithReadPreference(readPreference ?? ReadPreference.Primary)
            .GetCollection<T>(GetCollectionName());

    protected virtual IMongoCollection<T> Collection 
        => Database.GetCollection<T>(GetCollectionName());

    public Task CreateAsync(T entity) 
        => Collection.InsertOneAsync(entity);

    public Task UpdateAsync(T entity)
    {
        Expression<Func<T, string>> func = f => f.Id;
        var values = entity.GetType()
            .GetProperty(func.Body.ToString().Split(".")[1])
            ?.GetValue(entity, null)?.ToString();

        var filter = Builders<T>.Filter.Eq(func, values);

        return Collection.ReplaceOneAsync(filter, entity);
    }

    public Task DeleteAsync(string id) 
        => Collection.DeleteOneAsync(x => x.Id.Equals(id));

    private static string GetCollectionName()
    {
        return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName
            ?? throw new NotImplementedException($"BsonCollectionAttribute is not applied to the ${nameof(T)}");
    }
}
