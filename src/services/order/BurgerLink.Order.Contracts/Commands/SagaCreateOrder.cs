namespace BurgerLink.Order.Contracts.Commands;

public class SagaCreateOrder
{
    public Guid OrderId { get; set; }
    public string OrderName { get; set; }

    public Uri StatusUpdateAddress { get; set; }
}