using BurgerLink.Inventory.Entity;
using BurgerLink.Inventory.Services;
using MassTransit;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Activities.DecrementItemInventory;

public class
    DecrementItemInventoryActivity : IActivity<Contracts.Commands.DecrementItemInventory, DecrementItemInventoryLog>
{
    private readonly IInventoryService _inventoryService;

    public DecrementItemInventoryActivity(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<CompensationResult> Compensate(CompensateContext<DecrementItemInventoryLog> context)
    {
        var entity = await _inventoryService.InventoryEntityByItemName(context.Log.ItemName);
        if (entity == null)
        {
            return context.Compensated();
        }

        var filter = MongoDbFilters.InventoryFilterByName(context.Log.ItemName);
        var quantity = entity.Quantity + 1;
        await _inventoryService.Collection.UpdateOneAsync(
            filter,
            Builders<InventoryEntity>.Update.Set(inventoryEntity => inventoryEntity.Quantity, quantity));

        return context.Compensated();
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<Contracts.Commands.DecrementItemInventory> context)
    {
        var entity = await _inventoryService.InventoryEntityByItemName(context.Arguments.ItemName);
        if (entity == null || entity.Quantity < 1)
        {
            var vars = new List<KeyValuePair<string, object>>
            {
                new("unavailable-item", context.Arguments.ItemName)
            };

            return context
                .FaultedWithVariables(
                    new ArgumentOutOfRangeException(nameof(context.Arguments.ItemName)),
                    vars
                );
        }

        var filter = MongoDbFilters.InventoryFilterByName(context.Arguments.ItemName);
        var quantity = entity.Quantity - 1;
        await _inventoryService.Collection.UpdateOneAsync(
            filter,
            Builders<InventoryEntity>.Update.Set(inventoryEntity => inventoryEntity.Quantity, quantity));

        return context.Completed<DecrementItemInventoryLog>(new
        {
            context.Arguments.ItemName
        });
    }
}