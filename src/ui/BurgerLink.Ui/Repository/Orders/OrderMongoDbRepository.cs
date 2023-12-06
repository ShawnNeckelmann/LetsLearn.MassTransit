using BurgerLink.Shared.MongDbConfiguration;
using BurgerLink.Ui.Repository.Orders.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BurgerLink.Ui.Repository.Orders;

public class OrderMongoDbRepository : BaseMongoCollection<OrderItem> , IOrdersRepository
{
    public async Task<OrderItem?> Order(Guid orderId)
    {
        var filter = Builders<OrderItem>.Filter.Eq(order => order.Id, orderId);
        var retval = await Collection.FindAsync(filter);
        return retval?.Current.First();
    }

    public async Task<List<OrderItem>> AllOrders()
    {
        var retval = await Collection.Find(a => true).ToListAsync();
        return retval ?? new List<OrderItem>();
    }

    public async Task<OrderItem> SubmitOrderForConfirmation(OrderItem order)
    {
        order.ConfirmationStatus = "Pending";
        await Collection.InsertOneAsync(order);
        return order;
    }

    public async Task<OrderItem?> OrderConfirmed(Guid orderId)
    {
        var filter = Builders<OrderItem>.Filter.Eq(inventoryItem => inventoryItem.Id, orderId);
        var update = Builders<OrderItem>.Update
            .Set(orderItem => orderItem.ConfirmationStatus, "Confirmed");

        var options = new FindOneAndUpdateOptions<OrderItem>()
        {
            ReturnDocument = ReturnDocument.After
        };

        var updatedItem = await Collection.FindOneAndUpdateAsync(filter, update, options);
        return updatedItem ?? null;
    }

    public OrderMongoDbRepository(IOptions<MongoDbSettings> settings) : base(settings.Value)
    {
    }
}