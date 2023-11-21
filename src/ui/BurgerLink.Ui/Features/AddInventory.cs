using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;
using MediatR;

namespace BurgerLink.Ui.Features;

public class AddInventory
{
    public class HandlerAddInventory : IRequestHandler<RequestAddInventory, Unit>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public HandlerAddInventory(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(RequestAddInventory request, CancellationToken cancellationToken)
        {
            var msg = new UpsertInventoryItem
            {
                Id = null,
                Quantity = request.Quantity,
                ItemName = request.ItemName
            };

            await _publishEndpoint.Publish(msg, cancellationToken);

            return Unit.Value;
        }
    }

    public class RequestAddInventory : IRequest<Unit>
    {
        public string ItemName { get; init; }

        public int Quantity { get; set; }
    }
}