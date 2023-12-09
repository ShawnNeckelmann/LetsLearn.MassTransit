using BurgerLink.Ui.Repository.Orders;
using BurgerLink.Ui.Repository.Orders.Models;
using MediatR;

namespace BurgerLink.Ui.Features.Orders;

public class CreateOrder
{
    public class Command : IRequest<Response>
    {
        public string OrderName { get; set; } = string.Empty;
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
                Id = order.Id,
                ConfirmationStatus= order.ConfirmationStatus
            };

            return retval;
        }
    }

    public class Response : OrderItem
    {
        
    }
}