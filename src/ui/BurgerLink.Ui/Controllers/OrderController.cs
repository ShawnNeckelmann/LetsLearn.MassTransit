using BurgerLink.Ui.Features;
using BurgerLink.Ui.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Ui.Controllers;

public class OrderController : BaseController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> CreateOrder(CreateOrder.Command command)
    {
        await _mediator.Send(command);
        var retval = CreatedAtAction(
            nameof(GetOrderById),
            new { orderId = command.OrderId },
            command);

        return await Task.FromResult(retval);
    }

    [HttpGet]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public IActionResult GetOrderById(string orderId)
    {
        var o = new CreateOrder.Command
        {
            OrderId = orderId
        };

        return Ok(o);
    }
}