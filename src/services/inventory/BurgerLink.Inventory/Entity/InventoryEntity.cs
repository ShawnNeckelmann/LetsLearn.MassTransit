using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BurgerLink.Inventory.Entity;

public class InventoryEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string ItemName { get; set; }

    public int Quantity { get; set; }
}