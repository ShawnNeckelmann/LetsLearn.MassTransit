using BurgerLink.Ui.Repository.Orders;
using BurgerLink.Ui.Repository.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class SetOrderItems
{
    public class Command : IRequest<Response?>
    {
        public string OrderId { get; set; }

        public List<string> InventoryIds { get; set; }
    }

    public class Response : OrderItem
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
            var order = await _ordersRepository.SetOrderItems(request.OrderId, request.InventoryIds);
            if (order == null)
            {
                return null;
            }

            var retval = new Response();

            return retval;
        }
    }
}