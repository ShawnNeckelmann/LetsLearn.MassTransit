namespace BurgerLink.Order.Contracts;

public interface IngredientValidated
{
    string IngredientName { get; set; }
    string OrderName { get; set; }
}