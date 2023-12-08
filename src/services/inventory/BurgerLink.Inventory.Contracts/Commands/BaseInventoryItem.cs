namespace BurgerLink.Inventory.Contracts.Commands;

public abstract record BaseInventoryItem
{
    public string Id { get; set; }
    public string ItemName { get; init; }

    public int Quantity { get; set; }
}