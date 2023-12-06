namespace BurgerLink.Order.Contracts.Responses
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
    }
}
