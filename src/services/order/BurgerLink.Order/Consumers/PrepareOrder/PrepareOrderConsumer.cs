using BurgerLink.Order.Consumers.PreparationComplete;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace BurgerLink.Order.Consumers.PrepareOrder;

public class PrepareOrderConsumer : IConsumer<PrepareOrder>
{
    public async Task Consume(ConsumeContext<PrepareOrder> context)
    {
        var order = context.Message.Order;
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        // First remove all the items from inventory.
        foreach (var itemName in order.Items)
            builder.AddActivity(
                $"Decrement{itemName}",
                new Uri("queue:decrement-item-inventory_execute"),
                new
                {
                    itemName
                }
            );

        // Prepare all the items.
        foreach (var itemName in order.Items)
            builder.AddActivity(
                $"Decrement{itemName}",
                new Uri("queue:prepare-item_execute"),
                new
                {
                    itemName,
                    order.OrderName
                }
            );

        await builder.AddSubscription(
            context.SourceAddress,
            RoutingSlipEvents.Faulted,
            RoutingSlipEventContents.All,
            endpoint => endpoint.Send<ItemUnavailable>(new
            {
                context.Message.Order.OrderName
            })
        );

        await builder.AddSubscription(
            context.SourceAddress,
            RoutingSlipEvents.Completed,
            RoutingSlipEventContents.None,
            endpoint =>
            {
                Thread.Sleep(1000);
                return endpoint.Send<PreparationComplete.PreparationComplete>(new
                {
                    context.Message.Order.OrderName
                });
            });

        var slip = builder.Build();
        await context.Execute(slip);
    }
}