namespace BurgerLink.Order.Contracts.Commands;

public class AddIngredientToOrder
{
    public string IngredientName { get; set; }
    public string OrderName { get; set; }
}