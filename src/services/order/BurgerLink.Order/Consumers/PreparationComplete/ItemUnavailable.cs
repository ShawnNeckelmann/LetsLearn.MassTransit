using MassTransit.Courier.Contracts;

namespace BurgerLink.Order.Consumers.PreparationComplete;

public class ItemUnavailable : RoutingSlipFaulted
{
    public ActivityException[] ActivityExceptions { get; set; }
    public TimeSpan Duration { get; set; }
    public string OrderName { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid TrackingNumber { get; set; }
    public IDictionary<string, object> Variables { get; set; }
}