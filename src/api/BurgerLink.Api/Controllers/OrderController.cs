using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Api.Controllers;

public class OrderController : BaseController
{
    private readonly IRequestClient<AddIngredientToOrder> _addIngredientRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(
        IRequestClient<AddIngredientToOrder> addIngredientRequestClient,
        IPublishEndpoint publishEndpoint)
    {
        _addIngredientRequestClient = addIngredientRequestClient ??
                                      throw new ArgumentNullException(nameof(addIngredientRequestClient));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    [HttpPost("addIngredient")]
    public async Task<IActionResult> AddIngredient(AddIngredientToOrder addIngredientToOrder)
    {
        var response =
            await _addIngredientRequestClient.GetResponse<OrderUpdateAccepted, OrderNotFound>(addIngredientToOrder);
        return response.Is(out Response<OrderUpdateAccepted> result) ? Accepted() : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> StartOrder(CreateOrder createOrder)
    {
        await _publishEndpoint.Publish(createOrder);
        return Accepted();
    }
}