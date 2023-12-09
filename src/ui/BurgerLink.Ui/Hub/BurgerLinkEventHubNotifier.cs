using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository.Inventory.Models.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

public class BurgerLinkEventHubNotifier : BackgroundService, INotificationHandler<ItemAdded>,
    INotificationHandler<ItemModified>
{
    private readonly IHubContext<BurgerLinkEventHub> _context;

    public BurgerLinkEventHubNotifier(IHubContext<BurgerLinkEventHub> context)
    {
        _context = context;
    }

    public async Task Handle(ItemAdded notification, CancellationToken cancellationToken)
    {
        await _context.Clients.All.SendCoreAsync(
            nameof(InventoryItemAdded),
            new object?[] { notification }, cancellationToken);
    }

    public async Task Handle(ItemModified notification, CancellationToken cancellationToken)
    {
        await _context.Clients.All.SendCoreAsync(
            nameof(InventoryItemModified),
            new object?[] { notification }, cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
    }
}