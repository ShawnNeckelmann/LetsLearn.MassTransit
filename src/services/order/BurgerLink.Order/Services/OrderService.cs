using BurgerLink.Order.Entity;
using BurgerLink.Shared.MongDbConfiguration;
using Microsoft.Extensions.Options;

namespace BurgerLink.Order.Services;

public class OrderService : BaseMongoCollection<OrderEntity>, IOrderService
{
    public OrderService(IOptions<MongoDbSettings> options) : base(options.Value)
    {
    }
}