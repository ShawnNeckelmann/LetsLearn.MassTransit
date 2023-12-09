namespace BurgerLink.Inventory.Contracts.Requests;

public record GetInventoryItem
{
    public string ItemName { get; set; }
}