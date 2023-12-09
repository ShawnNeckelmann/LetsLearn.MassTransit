using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository.Inventory;
using BurgerLink.Ui.Repository.Inventory.Models;
using MassTransit;

namespace BurgerLink.Ui.Consumers;

public class InventoryConsumer : IConsumer<InventoryItemModified>, IConsumer<InventoryItemAdded>
{
    private readonly IInventoryRepository _inventoryRepository;


    public InventoryConsumer(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task Consume(ConsumeContext<InventoryItemAdded> context)
    {
        var msg = context.Message;
        var item = new InventoryItem
        {
            Id = msg.Id,
            ItemName = msg.ItemName,
            Quantity = msg.Quantity
        };

        await _inventoryRepository.AddInventoryItem(item);
    }

    public async Task Consume(ConsumeContext<InventoryItemModified> context)
    {
        var msg = context.Message;
        var item = new InventoryItem
        {
            Id = msg.Id,
            Quantity = msg.Quantity,
            ItemName = msg.ItemName
        };

        await _inventoryRepository.ModifyInventoryItem(item);
    }
}