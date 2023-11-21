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

    public async Task ModifyItem(InventoryItem item)
    {
        var filter = Builders<InventoryItem>.Filter.Eq(inventoryItem => inventoryItem.Id, item.Id);
        var update = Builders<InventoryItem>.Update
            .Set(inventoryItem => inventoryItem.ItemName, item.ItemName)
            .Set(inventoryItem => inventoryItem.Quantity, item.Quantity);

        await Collection.UpdateOneAsync(filter, update);
        OnItemModified?.Invoke(this, item);
    }

    public event EventHandler<InventoryItem>? OnItemAdded;
    public event EventHandler<InventoryItem>? OnItemModified;
}