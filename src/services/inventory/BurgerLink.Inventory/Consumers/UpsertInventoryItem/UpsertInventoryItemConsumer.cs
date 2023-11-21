using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Inventory.Entity;
using BurgerLink.Inventory.Services;
using MassTransit;
using MongoDB.Driver;

namespace BurgerLink.Inventory.Consumers.UpsertInventoryItem;

public class UpsertInventoryItemConsumer : IConsumer<Contracts.Commands.UpsertInventoryItem>
{
    private readonly IInventoryService _inventoryService;

    public UpsertInventoryItemConsumer(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task Consume(ConsumeContext<Contracts.Commands.UpsertInventoryItem> context)
    {
        var msg = context.Message;
        var id = msg.Id;

        if (id == null)
        {
            var item = new InventoryEntity
            {
                ItemName = msg.ItemName,
                Quantity = msg.Quantity
            };

            await _inventoryService.Collection.InsertOneAsync(item);

            await context.Publish(new InventoryItemAdded
            {
                Id = item.Id,
                ItemName = msg.ItemName,
                Quantity = msg.Quantity
            });
        }
        else
        {
            var filter = Builders<InventoryEntity>.Filter.Eq(inventoryEntity => inventoryEntity.Id, msg.Id);

            var update =
                Builders<InventoryEntity>.Update
                    .Set(entity => entity.Quantity, msg.Quantity)
                    .Set(entity => entity.ItemName, msg.ItemName);

            var options = new FindOneAndUpdateOptions<InventoryEntity>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedEntity = await _inventoryService.Collection.FindOneAndUpdateAsync(filter, update, options);
            if (updatedEntity?.Id is null)
            {
                return;
            }

            await context.Publish(new InventoryItemModified
            {
                Id = updatedEntity.Id,
                ItemName = updatedEntity.ItemName,
                Quantity = updatedEntity.Quantity
            });
        }
    }
}