namespace BurgerLink.Order.Contracts;

public interface ItemAvailabilityValidated
{
    string ItemName { get; set; }
    string OrderName { get; set; }
    IDictionary<string, object> Variables { get; }
}