using BurgerLink.Inventory.Entity;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Services;

public static class MongoDbFilters
{
    public static ExpressionFilterDefinition<InventoryEntity> InventoryFilter(string itemName)
    {
        return new ExpressionFilterDefinition<InventoryEntity>(inventoryEntity =>
            inventoryEntity.ItemName.ToLower() == itemName);
    }
}