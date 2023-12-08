using BurgerLink.Order.Consumers.AddItemToOrder;
using BurgerLink.Order.Consumers.PreparationComplete;
using BurgerLink.Order.Consumers.PrepareOrder;
using BurgerLink.Order.Contracts;
using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Requests;
using BurgerLink.Order.Contracts.Responses;
using BurgerLink.Preparation.Contracts.Commands;
using MassTransit;

namespace BurgerLink.Order.State;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        InstanceState(orderState => orderState.CurrentState);
        ConfigureEvents();
        ConfigureBehavior();
    }

    public Event<SagaBeginPreparation> EventBeginPreparation { get; set; }
    public Event<SagaCreateOrder> EventCreateOrder { get; }
    public Event<ItemPrepared> EventItemPrepard { get; set; }
    public Event<ItemUnavailable> EventItemUnavailable { get; }
    public Event<ItemAvailabilityValidated> EventItemValidated { get; set; }
    public Event<PreparationComplete> EventPreparationComplete { get; set; }

    public Event<SagaSetOrderItems> EventSetOrderItems { get; }
    public Event<SagaOrderStatusRequest> EventStatusRequest { get; set; }
    public MassTransit.State StatePreparing { get; set; }
    public MassTransit.State StateSubmitted { get; set; }
    public MassTransit.State StateValidatingItemRequest { get; set; }

    private static async Task AddItemRequest(BehaviorContext<OrderState, SagaSetOrderItems> obj)
    {
        await obj.GetPayload<ConsumeContext>().Publish<AddItemToOrder>(new
        {
            OrderName = obj.Message.OrderId,
            obj.Message.ItemName
        });
    }

    private void ConfigureBehavior()
    {
        Initially(
            When(EventCreateOrder)
                .TransitionTo(StateSubmitted)
                .Publish(context => new OrderCreated
                {
                    OrderId = context.Message.OrderId,
                    OrderName = context.Message.OrderName
                })
        );

        During(
            StateSubmitted,
            When(EventSetOrderItems)
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
                            Items = context.Saga.Items.ToList(),
                            OrderName = context.Saga.OrderName
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
                x.CorrelateById(context => context.Message.CorrelationId);
                x.InsertOnInitial = true;
                x.SetSagaFactory(context =>
                {
                    var retval = new OrderState
                    {
                        CorrelationId = Guid.NewGuid(),
                        OrderId = context.Message.OrderId,
                        OrderName = context.Message.OrderName,
                        Items = new List<string>(),
                        Prepared = new List<string>()
                    };

                    return retval;
                });
            }
        );

        Event(() => EventSetOrderItems, x =>
            {
                x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderId);
                x.OnMissingInstance(OnMissingInstance);
            }
        );

        Event(() => EventItemUnavailable,
            x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventItemValidated,
            x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventStatusRequest, x =>
        {
            x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName);
            x.OnMissingInstance(OnMissingInstance);
        });
        Event(() => EventBeginPreparation,
            x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventItemPrepard,
            x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
        Event(() => EventPreparationComplete,
            x => { x.CorrelateBy((state, context) => state.OrderName == context.Message.OrderName); });
    }

    private static async Task OnBeginPreparation(BehaviorContext<OrderState, SagaBeginPreparation> obj)
    {
        await obj.GetPayload<ConsumeContext>().Publish<PrepareOrder>(new
        {
            Order = obj.Saga
        });
    }

    private static async Task OnItemPrepared(BehaviorContext<OrderState, ItemPrepared> arg)
    {
        arg.Saga.Prepared.Add(arg.Message.PreparedOrderItem);
    }

    private static Task OnItemUnavailable(BehaviorContext<OrderState, ItemUnavailable> arg)
    {
        return Task.CompletedTask;
    }

    private static async Task OnItemValidated(BehaviorContext<OrderState, ItemAvailabilityValidated> obj)
    {
        var (_, value) = obj.Message.Variables.ToList().First(pair => pair.Key.Equals("Valid"));

        if (value is true)
        {
            obj.Saga.Items.Add(obj.Message.ItemName);
            //await SendStatusUpdate(obj.Message, obj.Saga.StatusUpdateAddress, true);
        }
        //await SendStatusUpdate(obj.Message, obj.Saga.StatusUpdateAddress, false);
    }

    private static IPipe<ConsumeContext<SagaSetOrderItems>> OnMissingInstance(
        IMissingInstanceConfigurator<OrderState, SagaSetOrderItems> m)
    {
        return m.ExecuteAsync(context => context.RespondAsync<OrderNotFound>(new
        {
            OrderName = context.Message.OrderId,
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

    private static Task OnOrderPrepared(BehaviorContext<OrderState, PreparationComplete> arg)
    {
        return Task.CompletedTask;
    }
}