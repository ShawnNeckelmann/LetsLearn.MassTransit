using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;
using MediatR;

namespace BurgerLink.Ui.Features;

public class AddInventoryItem
{
    public class Command : IRequest<Unit>
    {
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

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var msg = new UpsertInventoryItem
            {
                Id = null,
                Quantity = command.Quantity,
                ItemName = command.ItemName
            };

            await _publishEndpoint.Publish(msg, cancellationToken);

            return Unit.Value;
        }
    }
}