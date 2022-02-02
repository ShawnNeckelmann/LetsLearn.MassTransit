using BurgerLink.Inventory.Entity;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Services;

public interface IInventoryService
{
    IMongoCollection<InventoryEntity> Collection { get; set; }
}