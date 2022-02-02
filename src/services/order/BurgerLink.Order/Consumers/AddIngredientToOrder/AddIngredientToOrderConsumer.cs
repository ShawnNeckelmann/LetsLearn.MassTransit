using BurgerLink.Order.Contracts.Responses;
using BurgerLink.Order.Entity;
using BurgerLink.Order.Services;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MongoDB.Driver;

namespace BurgerLink.Order.Consumers.AddIngredientToOrder;

public class AddIngredientToOrderConsumer : IConsumer<Contracts.Commands.AddIngredientToOrder>
{
    private readonly IOrderService _orderService;

    public AddIngredientToOrderConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<Contracts.Commands.AddIngredientToOrder> context)
    {
        var filter = MongoDbFilters.OrderFilter(context.Message.OrderName);
        var entity = await _orderService.Collection
            .Find(filter)
            .SingleOrDefaultAsync();

        if (entity == null)
        {
            await context.RespondAsync<OrderNotFound>(new { });
            return;
        }

        await context.RespondAsync<OrderUpdateAccepted>(new { });

        // Set Order to validating
        await _orderService.Collection.UpdateOneAsync(filter,
            Builders<OrderEntity>.Update.Set(orderEntity => orderEntity.Validating, true)
        );

        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        builder.AddActivity(
            "IngredientExists",
            new Uri("queue:validate-ingredient_execute"),
            new
            {
                context.Message.IngredientName
            });

        builder.AddActivity(
            "IngredientValidated",
            new Uri("queue:ingredient-validated_execute"),
            new
            {
                context.Message.OrderName,
                context.Message.IngredientName,
            });

        await builder.AddSubscription(
            new Uri("queue:ingredient-validated"),
            RoutingSlipEvents.All,
            RoutingSlipEventContents.All,
            endpoint => endpoint.Send<Contracts.IngredientValidated>(new
            {
                context.Message.OrderName,
                context.Message.IngredientName,
            })
        );

        builder.AddVariable("Valid", false);
        builder.AddVariable("StatusUpdateAddress", entity.StatusUpdateAddress);

        var slip = builder.Build();

        await context.Execute(slip);
    }
}