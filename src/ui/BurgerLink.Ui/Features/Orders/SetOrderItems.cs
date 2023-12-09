using BurgerLink.Ui.Repository.Orders;
using BurgerLink.Ui.Repository.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class SetOrderItems
{
    public class Command : OrderItem, IRequest<Response?>
    {
        
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
            if (request.Id == null)
            {
                return null;
            }

            var order = await _ordersRepository.SetOrderItems(request.Id, request.OrderItemIds);
            if (order == null)
            {
                return null;
            }

            var retval = new Response();

            return retval;
        }
    }
}