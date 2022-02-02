using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace BurgerLink.Inventory.Consumers.UpsertInventoryItem;

public class UpsertInventoryItemConsumerDefinition :
    ConsumerDefinition<UpsertInventoryItemConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<UpsertInventoryItemConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}