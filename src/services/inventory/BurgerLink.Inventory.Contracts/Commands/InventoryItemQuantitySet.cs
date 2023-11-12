namespace BurgerLink.Inventory.Contracts.Commands;

public record InventoryItemQuantitySet
{
    public string ItemName { get; init; }

    public int Quantity { get; set; }
}