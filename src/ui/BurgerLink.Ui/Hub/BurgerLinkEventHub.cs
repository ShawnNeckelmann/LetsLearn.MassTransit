using Microsoft.AspNetCore.SignalR;

public class BurgerLinkEventHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("OnConnected", $"{Context.ConnectionId} has joined.");
    }
}