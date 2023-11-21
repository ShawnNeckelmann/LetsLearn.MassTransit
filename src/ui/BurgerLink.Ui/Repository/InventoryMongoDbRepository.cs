using BurgerLink.Shared.MongDbConfiguration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BurgerLink.Ui.Repository;

public class InventoryMongoDbRepository : BaseMongoCollection<InventoryItem>, IInventoryRepository
{
    public InventoryMongoDbRepository(IOptions<MongoDbSettings> settings) : base(settings.Value)
    {
    }


    public async Task<InventoryItem> AddInventoryItem(InventoryItem item)
    {
        await Collection.InsertOneAsync(item);
        OnItemAdded?.Invoke(this, item);
        return item;
    }

    public async Task<IEnumerable<InventoryItem>> AllInventoryItems()
    {
        var retval = await Collection.Find(item => true).ToListAsync();
        return retval ?? new List<InventoryItem>();
    }

    public event EventHandler<InventoryItem>? OnItemAdded;
}