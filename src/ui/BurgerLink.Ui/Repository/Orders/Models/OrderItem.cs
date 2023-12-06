using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BurgerLink.Ui.Repository.Orders.Models
{
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }
        public List<string> OrderItemIds { get; set; }
        public string OrderName { get; set; }

        public string ConfirmationStatus { get; set; }
    }
}
