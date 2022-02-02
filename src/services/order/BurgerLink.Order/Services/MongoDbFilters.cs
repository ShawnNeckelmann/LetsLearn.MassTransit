using BurgerLink.Order.Entity;
using MongoDB.Driver;

namespace BurgerLink.Order.Services;

public static class MongoDbFilters
{
    public static ExpressionFilterDefinition<OrderEntity> OrderFilter(string itemName)
    {
        return new ExpressionFilterDefinition<OrderEntity>(inventoryEntity =>
            inventoryEntity.OrderName.ToLower() == itemName);
    }
}