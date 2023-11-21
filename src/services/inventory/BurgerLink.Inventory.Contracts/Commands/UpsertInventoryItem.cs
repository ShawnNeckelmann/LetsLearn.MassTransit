namespace BurgerLink.Inventory.Contracts.Commands;

public record UpsertInventoryItem
{
    public string? Id { get; set; }
    public string ItemName { get; set; }

    public int Quantity { get; set; }
}