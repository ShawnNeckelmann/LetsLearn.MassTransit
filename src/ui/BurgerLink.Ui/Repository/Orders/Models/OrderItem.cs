using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BurgerLink.Ui.Repository.Orders.Models
{
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public List<string> OrderItemIds { get; set; } = new();
        public string OrderName { get; set; } = string.Empty;

        public string ConfirmationStatus { get; set; } = string.Empty;
    }
}
