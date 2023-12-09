using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BurgerLink.Order.Entity;

public class OrderEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public List<string> Items { get; set; }
    public string OrderName { get; set; }
    public Uri StatusUpdateAddress { get; set; }
    public bool Validating { get; set; }
}