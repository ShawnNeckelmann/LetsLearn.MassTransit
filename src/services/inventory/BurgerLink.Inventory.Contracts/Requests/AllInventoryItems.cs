using BurgerLink.Inventory.Contracts.Responses;

namespace BurgerLink.Inventory.Contracts.Requests;

public class AllInventoryItems
{
    public int Count { get; set; }
    public IList<InventoryItem> Items { get; set; }
}