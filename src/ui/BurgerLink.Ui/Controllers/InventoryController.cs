using BurgerLink.Ui.Features;
using BurgerLink.Ui.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Ui.Controllers;

public class InventoryController : BaseController
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var retval = await _mediator.Send(new GetInventory.GetInventoryRequest());
        return Ok(retval);
    }
}