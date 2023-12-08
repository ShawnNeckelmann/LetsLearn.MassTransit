﻿using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Requests;
using BurgerLink.Order.Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Api.Controllers;

public class OrderController : BaseController
{
    private readonly IRequestClient<SagaSetOrderItems> _addItemRequestClient;
    private readonly IRequestClient<SagaOrderStatusRequest> _orderStatusRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(
        IRequestClient<SagaSetOrderItems> addItemRequestClient,
        IRequestClient<SagaOrderStatusRequest> orderStatusRequestClient,
        IPublishEndpoint publishEndpoint)
    {
        _addItemRequestClient = addItemRequestClient ?? throw new ArgumentNullException(nameof(addItemRequestClient));
        _orderStatusRequestClient = orderStatusRequestClient ??
                                    throw new ArgumentNullException(nameof(orderStatusRequestClient));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    [HttpPost("addItem")]
    public async Task<IActionResult> AddItem(SagaSetOrderItems itemsToOrder)
    {
        var (accepted, notfound) =
            await _addItemRequestClient.GetResponse<OrderUpdateAccepted, OrderNotFound>(itemsToOrder);

        if (accepted.IsCompletedSuccessfully)
        {
            return Accepted();
        }

        await notfound;
        return NotFound(new
        {
            OrderName = itemsToOrder.OrderId
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(SagaCreateOrder sagaCreateOrder)
    {
        await _publishEndpoint.Publish(sagaCreateOrder);
        return Accepted();
    }

    [HttpGet]
    public async Task<IActionResult> OrderStatus([FromQuery] string orderName)
    {
        var (ok, notfound) = await _orderStatusRequestClient.GetResponse<OrderStatus, OrderNotFound>(new
        {
            OrderName = orderName
        });

        if (notfound.IsCompletedSuccessfully)
        {
            await notfound;
            return NotFound(new
            {
                orderName
            });
        }

        var response = await ok;
        return Ok(response.Message);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartOrderPreparation(SagaBeginPreparation beginPreparation)
    {
        await _publishEndpoint.Publish(beginPreparation);
        return Accepted();
    }
}