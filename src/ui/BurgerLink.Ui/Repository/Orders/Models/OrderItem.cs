using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BurgerLink.Ui.Repository.Orders.Models;

public class OrderItem
{
    public string ConfirmationStatus { get; set; } = string.Empty;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public List<string> OrderItemIds { get; set; } = new();
    public string OrderName { get; set; } = string.Empty;
}