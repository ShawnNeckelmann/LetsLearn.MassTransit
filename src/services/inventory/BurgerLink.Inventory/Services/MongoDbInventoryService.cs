using BurgerLink.Inventory.Entity;
using BurgerLink.Shared.MongDbConfiguration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Services;

public class MongoDbInventoryService : BaseMongoCollection<InventoryEntity>, IInventoryService
{
    public MongoDbInventoryService(IOptions<MongoDbSettings> options) : base(options.Value)
    {
    }

    public async Task<InventoryEntity> InventoryEntityByItemName(string itemName)
    {
        var filter = MongoDbFilters.InventoryFilterByName(itemName);
        var entity = await Collection
            .Find(filter)
            .SingleOrDefaultAsync();

        return entity;
    }
}