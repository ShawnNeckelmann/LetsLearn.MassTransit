using MediatR;

namespace BurgerLink.Ui.Features;

public class ModifyInventoryItem
{
    public class RequestModifyInventory : IRequest<Unit>
    {
        public string Id { get; set; }
        public string ItemName { get; init; }

        public int Quantity { get; set; }
    }
}