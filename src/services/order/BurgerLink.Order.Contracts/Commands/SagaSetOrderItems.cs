namespace BurgerLink.Order.Contracts.Commands;

public class SagaSetOrderItems
{
    public string ItemName { get; set; }

    public string OrderId { get; set; }
}