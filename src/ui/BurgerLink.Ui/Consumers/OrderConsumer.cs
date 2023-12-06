using BurgerLink.Order.Contracts.Responses;
using BurgerLink.Ui.Repository.Orders;
using MassTransit;

namespace BurgerLink.Ui.Consumers;

public class OrderConsumer : IConsumer<OrderCreated>
{
    private readonly IOrdersRepository _ordersRepository;

    public OrderConsumer(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var msg = context.Message;
        await _ordersRepository.OrderConfirmed(msg.OrderId);
    }
}