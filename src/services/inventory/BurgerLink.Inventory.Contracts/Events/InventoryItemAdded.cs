using BurgerLink.Inventory.Contracts.Commands;

namespace BurgerLink.Inventory.Contracts.Events;

public record InventoryItemAdded : BaseInventoryItem
{
    public string ItemName { get; init; }

    public int Quantity { get; set; }
}