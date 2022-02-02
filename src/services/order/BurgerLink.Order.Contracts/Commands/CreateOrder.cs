namespace BurgerLink.Order.Contracts.Commands;

public class CreateOrder
{
    public string OrderName { get; set; }

    public Uri StatusUpdateAddress { get; set; }
}