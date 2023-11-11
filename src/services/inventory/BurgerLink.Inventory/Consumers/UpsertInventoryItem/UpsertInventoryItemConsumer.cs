using BurgerLink.Inventory.Contracts.Commands;
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
        var filter = MongoDbFilters.InventoryFilter(context.Message.ItemName);

        var entity = await _inventoryService.Collection.Find(filter).SingleOrDefaultAsync();

        if (entity == null)
        {
            var item = new InventoryEntity
            {
                ItemName = context.Message.ItemName,
                Quantity = context.Message.Quantity
            };

            await _inventoryService.Collection.InsertOneAsync(item);
            await context.Publish<InventoryItemSet>(new
            {
                context.Message.ItemName,
                context.Message.Quantity
            });
        }
        else
        {
            var quantity = context.Message.Quantity + entity.Quantity;

            await _inventoryService.Collection.UpdateOneAsync(
                Builders<InventoryEntity>.Filter.Eq(inventoryEntity => inventoryEntity.ItemName,
                    context.Message.ItemName),
                Builders<InventoryEntity>.Update.Set(inventoryEntity => inventoryEntity.Quantity, quantity)
            );

            await context.Publish<InventoryItemSet>(new
            {
                context.Message.ItemName,
                quantity
            });
        }
    }
}