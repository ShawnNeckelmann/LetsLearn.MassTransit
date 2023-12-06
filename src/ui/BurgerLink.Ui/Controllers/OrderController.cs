﻿using BurgerLink.Ui.Features.Orders;
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
        var order = await _mediator.Send(command);
        var retval = CreatedAtAction(
            nameof(GetOrderById),
            new { orderId = order.OrderId },
            command);

        return await Task.FromResult(retval);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Unit), 200)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var order = new GetOrder.Command
        {
            OrderId = orderId
        };

        var retval = await _mediator.Send(order);

        if (retval is null)
        {
            return NotFound();
        }

        return Ok(retval);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetOrders.Response), 200)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetOrders()
    {
        var command = new GetOrders.Command();
        var retval = await _mediator.Send(command);
        return Ok(retval);
    }
}