namespace BurgerLink.Inventory.Contracts.Commands;

public record UpsertInventoryItem
{
    public string ItemName { get; init; }

    public int Quantity { get; set; }
}