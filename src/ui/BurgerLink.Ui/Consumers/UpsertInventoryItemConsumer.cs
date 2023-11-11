using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;

namespace BurgerLink.Ui.Consumers;

public class UpsertInventoryItemConsumer : IConsumer<UpsertInventoryItem>
{
    public Task Consume(ConsumeContext<UpsertInventoryItem> context)
    {
        Console.WriteLine(context.Message.ItemName);
        return Task.CompletedTask;
    }
}