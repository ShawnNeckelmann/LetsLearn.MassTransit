﻿using BurgerLink.Inventory.Contracts.Events;
using BurgerLink.Ui.Repository;
using MassTransit;

namespace BurgerLink.Ui.Consumers;

public class InventoryModifiedConsumer : IConsumer<InventoryItemModified>, IConsumer<InventoryItemAdded>
{
    private readonly IInventoryRepository _inventoryRepository;


    public InventoryModifiedConsumer(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task Consume(ConsumeContext<InventoryItemAdded> context)
    {
        var msg = context.Message;
        var item = new InventoryItem
        {
            Id = msg.Id,
            ItemName = msg.ItemName,
            Quantity = msg.Quantity
        };

        await _inventoryRepository.AddInventoryItem(item);
    }

    public async Task Consume(ConsumeContext<InventoryItemModified> context)
    {
        var msg = context.Message;
        var item = new InventoryItem
        {
            Id = msg.Id,
            Quantity = msg.Quantity,
            ItemName = msg.ItemName
        };

        await _inventoryRepository.ModifyInventoryItem(item);
    }
}