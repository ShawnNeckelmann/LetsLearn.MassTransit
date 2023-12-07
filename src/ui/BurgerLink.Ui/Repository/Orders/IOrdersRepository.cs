using BurgerLink.Ui.Repository.Orders.Models;

namespace BurgerLink.Ui.Repository.Orders
{
    public interface IOrdersRepository
    {
        Task<OrderItem?> Order(string orderId);

        Task<List<OrderItem>> AllOrders();

        Task<OrderItem> SubmitOrderForConfirmation(string orderName);

        Task<OrderItem?> OrderSubmitted(string orderId);
    }
}
