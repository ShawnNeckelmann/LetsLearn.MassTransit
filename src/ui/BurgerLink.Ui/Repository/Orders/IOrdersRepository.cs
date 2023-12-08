using BurgerLink.Ui.Repository.Orders.Models;

namespace BurgerLink.Ui.Repository.Orders;

public interface IOrdersRepository
{
    Task<List<OrderItem>> AllOrders();
    Task<OrderItem?> Order(string orderId);

    Task<OrderItem?> OrderSubmitted(string orderId);

    Task<OrderItem> SubmitOrderForConfirmation(string orderName);
    Task<OrderItem?> SetOrderItems(string requestOrderId, List<string> requestInventoryIds);
}