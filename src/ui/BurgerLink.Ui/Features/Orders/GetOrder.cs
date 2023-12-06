using BurgerLink.Ui.Repository.Orders;
using BurgerLink.Ui.Repository.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class GetOrder
{
    public class Command : IRequest<Response?>
    {
        public Guid OrderId { get; set; }
    }

    public class Response : OrderItem
    {

    }

    public class Handler : IRequestHandler<Command, Response?>
    {
        private readonly IOrdersRepository _ordersRepository;

        public Handler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Response?> Handle(Command request, CancellationToken cancellationToken)
        {

            var order = await _ordersRepository.Order(request.OrderId);

        }
    }
}