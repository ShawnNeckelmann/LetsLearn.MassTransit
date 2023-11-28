using BurgerLink.Ui.Features;
using BurgerLink.Ui.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Ui.Controllers;

public class ExternalsController : BaseController
{
    private readonly ILogger<ExternalsController> _logger;
    private readonly IMediator _mediator;

    public ExternalsController(IMediator mediator, ILogger<ExternalsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Received request to retrieve an External Item {id}", Guid.NewGuid());
        var retval = await _mediator.Send(new GetExternals.GetExternalsRequest());
        return Ok(retval);
    }
}