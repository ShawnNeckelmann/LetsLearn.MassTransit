using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository;
using MassTransit;

namespace BurgerLink.Ui.Consumers;

public class InventoryModifiedConsumer : IConsumer<InventoryItemModified>, IConsumer<InventoryItemAdded>
{
    private readonly IInventoryRepository _inventoryRepository;


    public InventoryModifiedConsumer(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task Consume(ConsumeContext<InventoryItemAdded> context)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task Consume(ConsumeContext<InventoryItemModified> context)
    {
        Console.WriteLine(context.Message.ItemName);

        return Task.CompletedTask;
    }
}