using BurgerLink.Inventory.Contracts.Commands;
using BurgerLink.Inventory.Contracts.Requests;
using BurgerLink.Inventory.Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Api.Controllers;

public class InventoryController : BaseController
{
    private readonly IRequestClient<GetAllInventoryItems> _getAllInventoryItems;
    private readonly IRequestClient<GetInventoryItem> _getItemRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public InventoryController(
        IPublishEndpoint publishEndpoint,
        IRequestClient<GetInventoryItem> getItemRequestClient,
        IRequestClient<GetAllInventoryItems> getAllInventoryItems)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _getItemRequestClient = getItemRequestClient ?? throw new ArgumentNullException(nameof(getItemRequestClient));
        _getAllInventoryItems = getAllInventoryItems ?? throw new ArgumentNullException(nameof(getAllInventoryItems));
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(UpsertInventoryItem upsertInventoryItem)
    {
        await _publishEndpoint.Publish(upsertInventoryItem);
        return Accepted();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllItems()
    {
        var response = await _getAllInventoryItems.GetResponse<AllInventoryItems>(new { });
        return Ok(response.Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetItem(string itemName)
    {
        var response = await _getItemRequestClient.GetResponse<InventoryItem, InventoryItemNotFound>(new
        {
            ItemName = itemName
        });

        return response.Is(out Response<InventoryItem> result) ? Ok(result.Message) : NotFound();
    }
}