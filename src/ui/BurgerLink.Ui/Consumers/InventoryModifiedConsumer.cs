using BurgerLink.Inventory.Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace BurgerLink.Ui.Consumers;

public class InventoryModifiedConsumer : IConsumer<InventoryItemModified>, IConsumer<InventoryItemAdded>
{
    private readonly IHubContext<BurgerLinkEventHub> _hubContext;

    public InventoryModifiedConsumer(IHubContext<BurgerLinkEventHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Consume(ConsumeContext<InventoryItemAdded> context)
    {
        Console.WriteLine(context.Message.ItemName);

        _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemAdded),
            new object?[] { context.Message },
            context.CancellationToken);

        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<InventoryItemModified> context)
    {
        Console.WriteLine(context.Message.ItemName);

        _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemModified),
            new object?[] { context.Message },
            context.CancellationToken);

        return Task.CompletedTask;
    }
}