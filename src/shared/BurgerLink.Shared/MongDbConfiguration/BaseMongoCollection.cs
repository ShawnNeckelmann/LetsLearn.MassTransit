using MongoDB.Driver;

namespace BurgerLink.Shared.MongDbConfiguration;

public abstract class BaseMongoCollection<TCollection> where TCollection : class
{
    protected BaseMongoCollection(MongoDbSettings settings)
    {
        var mongoClient = new MongoClient(settings.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);

        Collection = mongoDatabase.GetCollection<TCollection>(settings.CollectionName);
    }

    public IMongoCollection<TCollection> Collection { get; set; }
}