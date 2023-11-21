using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository;
using Microsoft.AspNetCore.SignalR;

public class BurgerLinkEventHub : Hub
{
    private readonly IHubContext<BurgerLinkEventHub> _hubContext;
    private readonly IInventoryRepository _inventoryRepository;

    public BurgerLinkEventHub(IHubContext<BurgerLinkEventHub> hubContext, IInventoryRepository inventoryRepository)
    {
        _hubContext = hubContext;
        _inventoryRepository = inventoryRepository;

        _inventoryRepository.OnItemAdded += OnItemAdded;
        _inventoryRepository.OnItemModified += OnItemModified;
    }

    ~BurgerLinkEventHub()
    {
        _inventoryRepository.OnItemAdded += OnItemAdded;
        _inventoryRepository.OnItemModified += OnItemModified;
    }

    private async void OnItemAdded(object? sender, InventoryItem e)
    {
        await _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemAdded),
            new object?[] { e });
    }

    private async void OnItemModified(object? sender, InventoryItem e)
    {
        await _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemModified),
            new object?[] { e });
    }
}