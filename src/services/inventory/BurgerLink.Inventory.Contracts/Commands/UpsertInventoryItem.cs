namespace BurgerLink.Inventory.Contracts.Commands;

public record UpsertInventoryItem
{
    public string? Id { get; set; }
    public string ItemName { get; init; }

    public int Quantity { get; set; }
}