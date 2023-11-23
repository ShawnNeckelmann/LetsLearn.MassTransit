using BurgerLink.Inventory.Contracts.Commands;
using MassTransit;
using MediatR;

namespace BurgerLink.Ui.Features;

public class HandlerModifyInventory : IRequestHandler<ModifyInventoryItem.RequestModifyInventory, Unit>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public HandlerModifyInventory(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }


    public async Task<Unit> Handle(ModifyInventoryItem.RequestModifyInventory request,
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

public class AddInventoryItem
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