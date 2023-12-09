namespace BurgerLink.Order.Contracts.Commands;

public class SagaCreateOrder
{
    public Guid CorrelationId { get; set; }
    public string OrderId { get; set; }
    public string OrderName { get; set; }
}