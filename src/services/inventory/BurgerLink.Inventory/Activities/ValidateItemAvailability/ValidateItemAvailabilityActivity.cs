using BurgerLink.Inventory.Services;
using MassTransit;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Activities.ValidateItemAvailability;

public class ValidateItemAvailabilityActivity : IExecuteActivity<Contracts.Commands.ValidateItemAvailability>
{
    private readonly IInventoryService _inventoryService;

    public ValidateItemAvailabilityActivity(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<Contracts.Commands.ValidateItemAvailability> context)
    {
        var filter = MongoDbFilters.InventoryFilterByName(context.Arguments.ItemName);
        var entity = await _inventoryService.Collection
            .Find(filter)
            .SingleOrDefaultAsync();

        return context.CompletedWithVariables(new
        {
            Valid = entity != null
        });
    }
}