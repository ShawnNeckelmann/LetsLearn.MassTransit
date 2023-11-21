using BurgerLink.Inventory.Entity;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Services;

public static class MongoDbFilters
{
    public static ExpressionFilterDefinition<InventoryEntity> InventoryFilterByName(string itemName)
    {
        return new ExpressionFilterDefinition<InventoryEntity>(inventoryEntity =>
            inventoryEntity.ItemName.ToLower() == itemName);
    }
}