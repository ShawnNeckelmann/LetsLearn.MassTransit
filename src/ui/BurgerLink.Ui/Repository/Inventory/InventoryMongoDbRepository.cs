using BurgerLink.Shared.MongDbConfiguration;
using BurgerLink.Ui.Repository.Inventory.Models;
using BurgerLink.Ui.Repository.Inventory.Models.Events;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BurgerLink.Ui.Repository.Inventory;

public class InventoryMongoDbRepository : BaseMongoCollection<InventoryItem>, IInventoryRepository
{
    private readonly IMediator _mediator;

    public InventoryMongoDbRepository(IOptions<InventorySettings> settings, IMediator mediator) : base(settings.Value)
    {
        _mediator = mediator;
    }


    public async Task<InventoryItem> AddInventoryItem(InventoryItem item)
    {
        await Collection.InsertOneAsync(item);
        await _mediator.Publish(new ItemAdded(item));
        return item;
    }

    public async Task<IEnumerable<InventoryItem>> AllInventoryItems()
    {
        var retval = await Collection.Find(item => true).ToListAsync();
        return retval ?? new List<InventoryItem>();
    }

    public async Task ModifyInventoryItem(InventoryItem item)
    {
        var filter = Builders<InventoryItem>.Filter.Eq(inventoryItem => inventoryItem.Id, item.Id);
        var update = Builders<InventoryItem>.Update
            .Set(inventoryItem => inventoryItem.ItemName, item.ItemName)
            .Set(inventoryItem => inventoryItem.Quantity, item.Quantity);


        var options = new FindOneAndUpdateOptions<InventoryItem>
        {
            ReturnDocument = ReturnDocument.After
        };

        var updatedItem = await Collection.FindOneAndUpdateAsync(filter, update, options);
        if (updatedItem == null)
        {
            return;
        }

        await _mediator.Publish(new ItemModified(item));
    }
}