using BurgerLink.Ui.Repository.Orders;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class CreateOrder
{
    public class Command : IRequest<Response>
    {
        public string OrderName { get; set; }
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
            var order = await _ordersRepository.SubmitOrderForConfirmation(request.OrderName);
            var retval = new Response
            {
                OrderName = order.OrderName,
                OrderId = order.Id,
                OrderStatus = order.ConfirmationStatus
            };

            return retval;
        }
    }

    public class Response
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
    }
}