using BurgerLink.Ui.Repository;
using MediatR;

namespace BurgerLink.Ui.Features;

public class GetInventory
{
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public Handler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var inventoryItems = (await _inventoryRepository.AllInventoryItems()).ToList();
            var retval = new Response
            {
                InventoryItems = inventoryItems
            };

            return retval;
        }
    }

    public class Query : IRequest<Response>
    {
    }

    public class Response
    {
        public List<InventoryItem> InventoryItems { get; set; }
    }
}