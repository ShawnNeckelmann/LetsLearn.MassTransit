namespace BurgerLink.Inventory.Contracts.Commands;

public interface DecrementItemInventory
{
    string ItemName { get; set; }
}