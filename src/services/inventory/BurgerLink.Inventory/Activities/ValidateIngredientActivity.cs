using BurgerLink.Inventory.Contracts.Requests;
using BurgerLink.Inventory.Services;
using MassTransit.Courier;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Activities;

public class ValidateIngredientActivity : IExecuteActivity<ValidateIngredient>
{
    private readonly IInventoryService _inventoryService;

    public ValidateIngredientActivity(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ValidateIngredient> context)
    {
        var filter = MongoDbFilters.InventoryFilter(context.Arguments.IngredientName);
        var entity = await _inventoryService.Collection
            .Find(filter)
            .SingleOrDefaultAsync();

        return context.CompletedWithVariables(new
        {
            Valid = entity != null
        });
    }
}