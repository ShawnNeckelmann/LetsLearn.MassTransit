using BurgerLink.Ui.Repository.Orders;
using BurgerLink.Ui.Repository.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class GetOrders
{
    public class Command : IRequest<Response>
    {
    }

    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IOrdersRepository _ordersRepository;

        public Handler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var orders = await _ordersRepository.AllOrders();
            var retval = new Response
            {
                Orders = orders,
                Count = orders.Count
            };

            return retval;
        }
    }

    public class Response 
    {
        public int Count { get; set; }
        public IList<OrderItem> Orders { get; set; }
    }
}