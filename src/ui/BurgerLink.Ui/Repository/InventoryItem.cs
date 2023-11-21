namespace BurgerLink.Ui.Repository;

public record InventoryItem
{
    public string Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }
}