using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;
using MediatR;

namespace BurgerLink.Ui.Features.Inventory;

public class ModifyInventoryItem
{
    public class Command : IRequest<Unit>
    {
        public string Id { get; set; }
        public string ItemName { get; init; }

        public int Quantity { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }


        public async Task<Unit> Handle(Command request,
            CancellationToken cancellationToken)
        {
            var msg = new UpsertInventoryItem
            {
                Id = request.Id,
                ItemName = request.ItemName,
                Quantity = request.Quantity
            };

            await _publishEndpoint.Publish(msg, cancellationToken);
            return Unit.Value;
        }
    }
}