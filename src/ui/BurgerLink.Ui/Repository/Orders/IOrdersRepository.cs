using BurgerLink.Ui.Repository.Orders.Models;

namespace BurgerLink.Ui.Repository.Orders
{
    public interface IOrdersRepository
    {
        Task<OrderItem?> Order(Guid orderId);

        Task<List<OrderItem>> AllOrders();

        Task<OrderItem> SubmitOrderForConfirmation(OrderItem orderItem);

        Task<OrderItem?> OrderConfirmed(Guid orderId);
    }
}
