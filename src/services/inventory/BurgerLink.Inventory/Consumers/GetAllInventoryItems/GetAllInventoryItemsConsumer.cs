using BurgerLink.Inventory.Contracts.Requests;
using BurgerLink.Inventory.Contracts.Responses;
using BurgerLink.Inventory.Entity;
using BurgerLink.Inventory.Services;
using MassTransit;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Consumers.GetAllInventoryItems;

public class GetAllInventoryItemsConsumer : IConsumer<Contracts.Requests.GetAllInventoryItems>
{
    private readonly IInventoryService _inventoryService;

    public GetAllInventoryItemsConsumer(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task Consume(ConsumeContext<Contracts.Requests.GetAllInventoryItems> context)
    {
        var entities = await _inventoryService
            .Collection
            .Find(Builders<InventoryEntity>.Filter.Empty)
            .ToListAsync();

        if (entities == null)
        {
            await context.RespondAsync<AllInventoryItems>(new
            {
                Items = new List<InventoryItem>(),
                Count = 0
            });
            return;
        }

        var retval = new AllInventoryItems
        {
            Count = entities.Count,
            Items = new List<InventoryItem>()
        };

        foreach (var entity in entities)
            retval.Items.Add(new InventoryItem
            {
                ItemName = entity.ItemName,
                Quantity = entity.Quantity
            });

        await context.RespondAsync(retval);
    }
}