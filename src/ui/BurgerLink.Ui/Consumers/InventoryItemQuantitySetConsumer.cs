using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace BurgerLink.Ui.Consumers;

public class InventoryItemQuantitySetConsumer : IConsumer<InventoryItemQuantitySet>
{
    private readonly IHubContext<BurgerLinkEventHub> _hubContext;

    public InventoryItemQuantitySetConsumer(IHubContext<BurgerLinkEventHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Consume(ConsumeContext<InventoryItemQuantitySet> context)
    {
        Console.WriteLine(context.Message.ItemName);

        _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemQuantitySet),
            new object?[] { context.Message },
            context.CancellationToken);

        return Task.CompletedTask;
    }
}