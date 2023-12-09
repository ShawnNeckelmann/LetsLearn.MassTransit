using MediatR;

namespace BurgerLink.Ui.Repository.Inventory.Models.Events;

public class ItemModified : INotification
{
    public ItemModified(InventoryItem inventoryItem)
    {
        Id = inventoryItem.Id;
        ItemName = inventoryItem.ItemName;
        Quantity = inventoryItem.Quantity;
    }

    public string? Id { get; set; }

    public string ItemName { get; set; }

    public int Quantity { get; set; }
}