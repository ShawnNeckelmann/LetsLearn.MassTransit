namespace BurgerLink.Inventory.Contracts.Commands;

public abstract record BaseInventoryItem
{
    public string Id { get; set; }
}