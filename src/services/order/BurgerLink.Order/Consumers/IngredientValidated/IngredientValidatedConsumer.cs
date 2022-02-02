using BurgerLink.Order.Entity;
using BurgerLink.Order.Services;
using MassTransit;
using MongoDB.Driver;

namespace BurgerLink.Order.Consumers.IngredientValidated;

public class IngredientValidatedConsumer : IConsumer<Contracts.IngredientValidated>
{
    private readonly IOrderService _orderService;

    public IngredientValidatedConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }


    public async Task Consume(ConsumeContext<Contracts.IngredientValidated> context)
    {
        var filter = MongoDbFilters.OrderFilter(context.Message.OrderName);
        var updateDefinition = Builders<OrderEntity>.Update.Set(entity => entity.Validating, false);
        await _orderService.Collection.UpdateOneAsync(filter, updateDefinition);
    }
}