namespace BurgerLink.Ui.Repository;

public interface IInventoryRepository
{
    Task<InventoryItem> AddInventoryItem(InventoryItem item);
    Task<IEnumerable<InventoryItem>> AllInventoryItems();
    Task ModifyInventoryItem(InventoryItem item);

    event EventHandler<InventoryItem> OnItemAdded;

    event EventHandler<InventoryItem> OnItemModified;
}