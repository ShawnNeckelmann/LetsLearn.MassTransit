using BurgerLink.Ui.Features;
using BurgerLink.Ui.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static BurgerLink.Ui.Features.AddInventoryItem;
using static BurgerLink.Ui.Features.ModifyInventoryItem;

namespace BurgerLink.Ui.Controllers;

public class InventoryController : BaseController
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> AddInventoryItem(RequestAddInventory addInventory)
    {
        await _mediator.Send(addInventory);
        return Accepted();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var retval = await _mediator.Send(new GetInventory.RequestGetInventory());
        return Ok(retval);
    }


    [HttpPut]
    public async Task<IActionResult> ModifyInventoryItem(RequestModifyInventory modifyInventory)
    {
        await _mediator.Send(modifyInventory);
        return Accepted();
    }
}