using BurgerLink.Shared.MongDbConfiguration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BurgerLink.Ui.Repository;

public interface IInventoryRepository
{
    Task<InventoryItem> AddInventoryItem(InventoryItem item);
    Task<IEnumerable<InventoryItem>> AllInventoryItems();
}

public class InventoryMongoDbRepository : BaseMongoCollection<InventoryItem>, IInventoryRepository
{
    public InventoryMongoDbRepository(IOptions<MongoDbSettings> settings) : base(settings.Value)
    {
    }


    public async Task<InventoryItem> AddInventoryItem(InventoryItem item)
    {
        item.Id = string.Empty;
        await Collection.InsertOneAsync(item);
        return item;
    }

    public async Task<IEnumerable<InventoryItem>> AllInventoryItems()
    {
        var retval = await Collection.Find(item => true).ToListAsync();
        return retval ?? new List<InventoryItem>();
    }
}