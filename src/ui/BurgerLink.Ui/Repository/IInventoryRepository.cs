namespace BurgerLink.Ui.Repository;

public interface IInventoryRepository
{
    Task<InventoryItem> AddInventoryItem(InventoryItem item);
    Task<IEnumerable<InventoryItem>> AllInventoryItems();

    event EventHandler<InventoryItem> OnItemAdded;
}