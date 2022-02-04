namespace BurgerLink.Order.Contracts.Responses;

public class OrderStatus
{
    public List<string> Items { get; set; }
    public string OrderName { get; set; }
    public Uri? StatusUpdateAddress { get; set; }
}