using BurgerLink.Order.Contracts;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace BurgerLink.Order.Consumers.AddItemToOrder;

public class AddItemToOrderConsumer : IConsumer<AddItemToOrder>
{
    public async Task Consume(ConsumeContext<AddItemToOrder> context)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        builder.AddActivity(
            "IngredientExists",
            new Uri("queue:validate-item-availability_execute"),
            new
            {
                context.Message.ItemName
            });

        await builder.AddSubscription(
            context.SourceAddress,
            RoutingSlipEvents.Completed,
            RoutingSlipEventContents.Variables,
            endpoint => endpoint.Send<ItemAvailabilityValidated>(new
            {
                context.Message.OrderName,
                context.Message.ItemName
            })
        );

        builder.AddVariable("Valid", false);

        var slip = builder.Build();

        await context.Execute(slip);
    }
}