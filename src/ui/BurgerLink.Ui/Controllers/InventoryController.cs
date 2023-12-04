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

    [HttpPost]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> AddInventoryItem(AddInventoryItem.Command addInventory)
    {
        await _mediator.Send(addInventory);
        return Accepted();
    }

    [HttpGet]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> Get()
    {
        var retval = await _mediator.Send(new GetInventory.Query());
        return Ok(retval);
    }


    [HttpPut]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<IActionResult> ModifyInventoryItem(ModifyInventoryItem.Command modifyInventory)
    {
        await _mediator.Send(modifyInventory);
        return Accepted();
    }
}