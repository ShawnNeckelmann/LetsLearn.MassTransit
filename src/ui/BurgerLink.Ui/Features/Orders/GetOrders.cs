using BurgerLink.Ui.Repository.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class GetOrders
{
    public class Command : IRequest<Response>
    {

    }
    public class Response
    {
        public IList<Order> Orders { get; set; }

        public int Count { get; set; }
    }

    public class Handler : IRequestHandler<Command, Response>
    {
        public Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var retval = new Response
            {
                Orders = Enumerable.Range(1, 10).Select(i => Order.RandomGenerated()).ToList()

            };
            retval.Count = retval.Orders.Count;

            return Task.FromResult(retval);

        }
    }

}
