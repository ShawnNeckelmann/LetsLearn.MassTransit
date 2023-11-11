using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace BurgerLink.Ui.Consumers;

public class UpsertInventoryItemConsumer : IConsumer<InventoryItemSet>
{
    private readonly IHubContext<BurgerLinkEventHub> _hubContext;

    public UpsertInventoryItemConsumer(IHubContext<BurgerLinkEventHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Consume(ConsumeContext<InventoryItemSet> context)
    {
        Console.WriteLine(context.Message.ItemName);

        _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemSet),
            new object?[] { context.Message },
            context.CancellationToken);

        return Task.CompletedTask;
    }
}