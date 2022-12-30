using MassTransit;

namespace BurgerLink.Order.State;

public class OrderState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public List<string> Items { get; set; }
    public string OrderName { get; set; }
    public List<string> Prepared { get; set; }
    public Uri? StatusUpdateAddress { get; set; }
    public int Version { get; set; }
}