using BurgerLink.Ui.Features.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class GetOrder
{
    public class Command : IRequest<Response?>
    {
        public Guid OrderId { get; set; }
    }

    public class Response : Order
    {

    }

    public class Handler : IRequestHandler<Command, Response?>
    {
        public Task<Response?> Handle(Command request, CancellationToken cancellationToken)
        {
            var retval = new Response
            {
                OrderName = "random",
                OrderId = request.OrderId,
                OrderItemIds = new List<string>()
            };

            return Task.FromResult(retval);
        }
    }
}