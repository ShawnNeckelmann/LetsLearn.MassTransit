using BurgerLink.Inventory.Entity;
using BurgerLink.Shared.MongDbConfiguration;
using Microsoft.Extensions.Options;

namespace BurgerLink.Inventory.Services;

public class MongoDbInventoryService : BaseMongoCollection<InventoryEntity>, IInventoryService
{
    public MongoDbInventoryService(IOptions<MongoDbSettings> options) : base(options.Value)
    {
    }
}