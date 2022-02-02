using BurgerLink.Order.Entity;
using MongoDB.Driver;

namespace BurgerLink.Order.Services;

public interface IOrderService
{
    IMongoCollection<OrderEntity> Collection { get; set; }
}