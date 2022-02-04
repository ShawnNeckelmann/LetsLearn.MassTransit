namespace BurgerLink.Order.Contracts.Commands;

public class SagaModifyOrderAddItem
{
    public string ItemName { get; set; }

    public string OrderName { get; set; }
}