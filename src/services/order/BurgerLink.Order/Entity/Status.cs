namespace BurgerLink.Order.Entity;

public record Status
{
    public string IngredientName { get; set; }
    public string Message { get; set; }
    public string OrderName { get; set; }
}