using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository;
using Microsoft.AspNetCore.SignalR;

public class BurgerLinkEventHub : Hub
{
    private readonly IHubContext<BurgerLinkEventHub> _hubContext;

    public BurgerLinkEventHub(IHubContext<BurgerLinkEventHub> hubContext, IInventoryRepository inventoryRepository)
    {
        _hubContext = hubContext;
        inventoryRepository.OnItemAdded += InventoryRepositoryOnOnItemAdded;
    }

    private async void InventoryRepositoryOnOnItemAdded(object? sender, InventoryItem e)
    {
        await _hubContext.Clients.All.SendCoreAsync(
            nameof(InventoryItemAdded),
            new object?[] { e });
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("OnConnected", $"{Context.ConnectionId} has joined.");
    }
}