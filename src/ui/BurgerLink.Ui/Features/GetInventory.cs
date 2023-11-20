using MediatR;

namespace BurgerLink.Ui.Features;

public class GetInventory
{
    public class GetInventoryHandler : IRequestHandler<GetInventoryRequest, GetInventoryResponse>
    {
        public async Task<GetInventoryResponse> Handle(GetInventoryRequest request, CancellationToken cancellationToken)
        {
            var retval = new GetInventoryResponse
            {
                InventoryItems = new List<InventoryItem>
                {
                    new()
                    {
                        Quantity = 10,
                        Name = "ten",
                        Id = 1
                    },
                    new()
                    {
                        Quantity = 20,
                        Name = "twenty",
                        Id = 2
                    }
                }
            };


            return await Task.FromResult(retval);
        }
    }

    public class GetInventoryRequest : IRequest<GetInventoryResponse>
    {
    }

    public class GetInventoryResponse
    {
        public List<InventoryItem> InventoryItems { get; set; }
    }

    public record InventoryItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}