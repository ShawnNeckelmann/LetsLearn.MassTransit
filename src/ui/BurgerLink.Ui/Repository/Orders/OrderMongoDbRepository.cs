using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Shared.MongDbConfiguration;
using BurgerLink.Ui.Repository.Orders.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BurgerLink.Ui.Repository.Orders;

public class OrderMongoDbRepository : BaseMongoCollection<OrderItem>, IOrdersRepository
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderMongoDbRepository(IOptions<OrderSettings> settings, IPublishEndpoint publishEndpoint) : base(settings.Value)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<List<OrderItem>> AllOrders()
    {
        var retval = await Collection.Find(a => true).ToListAsync();
        return retval ?? new List<OrderItem>();
    }

    public async Task<OrderItem?> Order(string orderId)
    {
        var parsed = new ObjectId(orderId);
        var filter = Builders<OrderItem>.Filter.Eq("_id", parsed);
        var asyncCursor = await Collection.FindAsync(filter);
        var retval = asyncCursor.FirstOrDefault();
        return retval;
    }

    public async Task<OrderItem?> OrderSubmitted(string orderId)
    {
        var filter = Builders<OrderItem>.Filter.Eq(inventoryItem => inventoryItem.Id, orderId);
        var update = Builders<OrderItem>.Update
            .Set(orderItem => orderItem.ConfirmationStatus, "Submitted");

        var options = new FindOneAndUpdateOptions<OrderItem>
        {
            ReturnDocument = ReturnDocument.After
        };

        var updatedItem = await Collection.FindOneAndUpdateAsync(filter, update, options);
        return updatedItem ?? null;
    }

    public async Task<OrderItem> SubmitOrderForConfirmation(string orderName)
    {
        var order = new OrderItem
        {
            ConfirmationStatus = "Pending",
            OrderName = orderName,
            OrderItemIds = new List<string>()
        };

        await Collection.InsertOneAsync(order);
        await _publishEndpoint.Publish(new SagaCreateOrder
        {
            OrderName = orderName,
            OrderId = order.Id
        });

        return order;
    }
}