using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class CreateOrder
{
    public class Response
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
    }

    public class Command :  IRequest<Response>
    {
        public string OrderName { get; set; }
    }

    public class Handler : IRequestHandler<Command, Response>

    {
        

        public Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var retval = new Response
            {
                OrderId = Guid.NewGuid(),
                OrderName = request.OrderName
            };

            return Task.FromResult(retval);
        }
    }
}