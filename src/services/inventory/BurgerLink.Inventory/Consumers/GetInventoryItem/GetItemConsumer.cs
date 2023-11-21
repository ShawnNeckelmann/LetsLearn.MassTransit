using BurgerLink.Inventory.Contracts.Responses;
using BurgerLink.Inventory.Services;
using MassTransit;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Consumers.GetInventoryItem;

public class GetItemConsumer : IConsumer<Contracts.Requests.GetInventoryItem>
{
    private readonly IInventoryService _inventoryService;

    public GetItemConsumer(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task Consume(ConsumeContext<Contracts.Requests.GetInventoryItem> context)
    {
        var filter = MongoDbFilters.InventoryFilterByName(context.Message.ItemName);
        var entity = await _inventoryService.Collection
            .Find(filter)
            .SingleOrDefaultAsync();

        if (entity == null)
        {
            await context.RespondAsync<InventoryItemNotFound>(new
            {
            });
        }
        else
        {
            await context.RespondAsync<InventoryItem>(new
            {
                entity.ItemName,
                entity.Quantity
            });
        }
    }
}