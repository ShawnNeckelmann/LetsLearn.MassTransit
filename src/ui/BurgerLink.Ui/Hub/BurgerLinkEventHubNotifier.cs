using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository;
using Microsoft.AspNetCore.SignalR;

public class BurgerLinkEventHubNotifier : BackgroundService
{
    private readonly IHubContext<BurgerLinkEventHub> _context;
    private readonly IInventoryRepository _inventoryRepository;

    public BurgerLinkEventHubNotifier(IHubContext<BurgerLinkEventHub> context, IInventoryRepository inventoryRepository)
    {
        _context = context;
        _inventoryRepository = inventoryRepository;
        _inventoryRepository.OnItemAdded += OnItemAdded;
        _inventoryRepository.OnItemModified += OnItemModified;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
    }


    private async void OnItemAdded(object? sender, InventoryItem e)
    {
        await _context.Clients.All.SendCoreAsync(
            nameof(InventoryItemAdded),
            new object?[] { e });
    }

    private async void OnItemModified(object? sender, InventoryItem e)
    {
        await _context.Clients.All.SendCoreAsync(
            nameof(InventoryItemModified),
            new object?[] { e });
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _inventoryRepository.OnItemAdded += OnItemAdded;
        _inventoryRepository.OnItemModified += OnItemModified;
        return base.StopAsync(cancellationToken);
    }
}