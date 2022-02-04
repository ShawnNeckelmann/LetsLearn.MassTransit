using Automatonymous;
using BurgerLink.Order.Consumers.AddItemToOrder;
using BurgerLink.Order.Consumers.PreparationComplete;
using BurgerLink.Order.Consumers.PrepareOrder;
using BurgerLink.Order.Contracts;
using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Requests;
using BurgerLink.Order.Contracts.Responses;
using BurgerLink.Order.Entity;
using BurgerLink.Preparation.Contracts.Commands;
using GreenPipes;
using MassTransit;
using System.Net.Http.Json;
using System.Text.Json;

namespace BurgerLink.Order.State;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        InstanceState(orderState => orderState.CurrentState);
        ConfigureEvents();
        ConfigureBehavior();
    }

    public Event<SagaModifyOrderAddItem> EventAddItem { get; private set; }
    public Event<SagaBeginPreparation> EventBeginPreparation { get; set; }
    public Event<SagaCreateOrder> EventCreateOrder { get; private set; }
    public Event<ItemPrepared> EventItemPrepard { get; set; }
    public Event<ItemUnavailable> EventItemUnavailable { get; private set; }
    public Event<ItemAvailabilityValidated> EventItemValidated { get; set; }
    public Event<PreparationComplete> EventPreparationComplete { get; set; }
    public Event<SagaOrderStatusRequest> EventStatusRequest { get; set; }
    public Automatonymous.State StatePreparing { get; set; }
    public Automatonymous.State StateSubmitted { get; set; }
    public Automatonymous.State StateValidatingItemRequest { get; set; }

    private static async Task AddItemRequest(BehaviorContext<OrderState, SagaModifyOrderAddItem> obj)
    {
        await obj.GetPayload<ConsumeContext>().Publish<AddItemToOrder>(new
        {
            obj.Data.OrderName,
            obj.Data.ItemName
        });
    }

    private static async Task OnBeginPreparation(BehaviorContext<OrderState, SagaBeginPreparation> obj)
    {
        await obj.GetPayload<ConsumeContext>().Publish<PrepareOrder>(new
        {
            Order = obj.Instance
        });
    }

    private static async Task OnItemPrepared(BehaviorContext<OrderState, ItemPrepared> arg)
    {
        arg.Instance.Prepared.Add(arg.Data.PreparedOrderItem);

        if (arg.Instance.StatusUpdateAddress == null) return;

        var client = new HttpClient();
        client.BaseAddress = arg.Instance.StatusUpdateAddress;

        await client.PostAsJsonAsync(
            (string?)null,
            new
            {
                Message = "item prepared",
                arg.Data.PreparedOrderItem
            },
            new JsonSerializerOptions(JsonSerializerDefaults.Web),
            CancellationToken.None);
    }

    private static async Task OnItemUnavailable(BehaviorContext<OrderState, ItemUnavailable> arg)
    {
        var client = new HttpClient();
        client.BaseAddress = arg.Instance.StatusUpdateAddress;

        arg.Data.Variables.TryGetValue("unavailable-item", out var o);
        var item = o as string;

        await client.PostAsJsonAsync(
            (string?)null,
            new
            {
                Message = "item unavailable",
                Item = item,
                arg.Data.OrderName
            },
            new JsonSerializerOptions(JsonSerializerDefaults.Web),
            CancellationToken.None);
    }

    private static async Task OnItemValidated(BehaviorContext<OrderState, ItemAvailabilityValidated> obj)
    {
        var (_, value) = obj.Data.Variables.ToList().First(pair => pair.Key.Equals("Valid"));

        if (value is true)
        {
            obj.Instance.Items.Add(obj.Data.ItemName);
            await SendStatusUpdate(obj.Data, obj.Instance.StatusUpdateAddress, true);
        }
        else
        {
            await SendStatusUpdate(obj.Data, obj.Instance.StatusUpdateAddress, false);
        }
    }

    private static IPipe<ConsumeContext<SagaModifyOrderAddItem>> OnMissingInstance(
        IMissingInstanceConfigurator<OrderState, SagaModifyOrderAddItem> m)
    {
        return m.ExecuteAsync(context => context.RespondAsync<OrderNotFound>(new
        {
            context.Message.OrderName,
            InVar.Timestamp
        }));
    }

    private static IPipe<ConsumeContext<SagaOrderStatusRequest>> OnMissingInstance(
        IMissingInstanceConfigurator<OrderState, SagaOrderStatusRequest> arg)
    {
        return arg.ExecuteAsync(context => context.RespondAsync<OrderNotFound>(new
        {
            context.Message.OrderName,
            InVar.Timestamp
        }));
    }

    private static async Task OnOrderPrepared(BehaviorContext<OrderState, PreparationComplete> arg)
    {
        var uri = arg.Instance.StatusUpdateAddress;
        if (uri == null)
        {
            return;
        }

        var client = new HttpClient();
        client.BaseAddress = uri;
        await client.PostAsJsonAsync((string?)null, new
        {
            Message = $"order for {arg.Instance.OrderName} is ready",
            Timestamp = DateTime.UtcNow
        },
            new JsonSerializerOptions(JsonSerializerDefaults.Web),
            CancellationToken.None);
    }

    private static async Task SendStatusUpdate(ItemAvailabilityValidated itemValidated, Uri? uri, bool valid)
    {
        if (uri == null) return;

        var status = new Status
        {
            OrderName = itemValidated.OrderName,
            ItemName = itemValidated.ItemName,
            Message = valid ? "item added" : "invalid item provided"
        };

        var client = new HttpClient();
        client.BaseAddress = uri;
        await client.PostAsJsonAsync((string?)null, status, new JsonSerializerOptions(JsonSerializerDefaults.Web),
            CancellationToken.None);
    }

    private void ConfigureBehavior()
    {
        Initially(
            When(EventCreateOrder)
                .TransitionTo(StateSubmitted)
        );

        During(
            StateSubmitted,
            When(EventAddItem)
                .Respond(new OrderUpdateAccepted())
                .ThenAsync(AddItemRequest)
                .TransitionTo(StateValidatingItemRequest)
        );

        During(
            StateValidatingItemRequest,
            When(EventItemValidated)
                .ThenAsync(OnItemValidated)
                .TransitionTo(StateSubmitted)
        );

        During(
            StateSubmitted,
            When(EventBeginPreparation)
                .ThenAsync(OnBeginPreparation)
                .TransitionTo(StatePreparing)
        );

        During(
            StatePreparing,
            When(EventItemPrepard)
                .ThenAsync(OnItemPrepared),
            When(EventItemUnavailable)
                .ThenAsync(OnItemUnavailable)
                .TransitionTo(StateSubmitted),
            When(EventPreparationComplete)
                .ThenAsync(OnOrderPrepared)
                .Finalize()
        );

        DuringAny(
            When(EventStatusRequest)
                .RespondAsync(
                    async context => await context.Init<OrderStatus>(
                        new OrderStatus
                        {
                            Items = context.Instance.Items.ToList(),
                            OrderName = context.Instance.OrderName,
                            StatusUpdateAddress = context.Instance.StatusUpdateAddress
                        })
                )
        );
    }

    private void ConfigureEvents()
    {
        Event(
            () => EventCreateOrder,
            x =>
            {
                x.CorrelateById(context => context.Message.OrderId);
                x.InsertOnInitial = true;
                x.SetSagaFactory(context =>
                {
                    var retval = new OrderState
                    {
                        OrderName = context.Message.OrderName,
                        StatusUpdateAddress = context.Message.StatusUpdateAddress,
                        Items = new List<string>(),
                        Prepared = new List<string>()
                    };

                    return retval;
                });
            }
        );

        Event(() => EventAddItem, x =>
            {
                x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName);
                x.OnMissingInstance(OnMissingInstance);
            }
        );

        Event(() => EventItemUnavailable, x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventItemValidated, x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventStatusRequest, x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); x.OnMissingInstance(OnMissingInstance); });
        Event(() => EventBeginPreparation, x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventItemPrepard, x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventPreparationComplete, x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
    }
}