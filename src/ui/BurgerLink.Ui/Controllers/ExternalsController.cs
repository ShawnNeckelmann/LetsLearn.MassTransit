using BurgerLink.Ui.Features;
using BurgerLink.Ui.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Ui.Controllers;

public class ExternalsController : BaseController
{
    private readonly IMediator _mediator;

    public ExternalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var retval = await _mediator.Send(new GetExternals.GetExternalsRequest());
        return Ok(retval);
    }
}