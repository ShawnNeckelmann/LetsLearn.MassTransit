using BurgerLink.Ui.Repository;
using MediatR;

namespace BurgerLink.Ui.Features;

public class GetInventory
{
    public class HandlerGetInventory : IRequestHandler<RequestGetInventory, ResponseGetInventory>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public HandlerGetInventory(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ResponseGetInventory> Handle(RequestGetInventory request, CancellationToken cancellationToken)
        {
            var inventoryItems = (await _inventoryRepository.AllInventoryItems()).ToList();
            var retval = new ResponseGetInventory
            {
                InventoryItems = inventoryItems
            };

            return retval;
        }
    }

    public class RequestGetInventory : IRequest<ResponseGetInventory>
    {
    }

    public class ResponseGetInventory
    {
        public List<InventoryItem> InventoryItems { get; set; }
    }
}