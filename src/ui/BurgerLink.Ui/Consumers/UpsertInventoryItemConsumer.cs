using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;

namespace BurgerLink.Ui.Consumers;

public class UpsertInventoryItemConsumer : IConsumer<InventoryItemSet>
{
    public Task Consume(ConsumeContext<InventoryItemSet> context)
    {
        Console.WriteLine(context.Message.ItemName);
        return Task.CompletedTask;
    }
}