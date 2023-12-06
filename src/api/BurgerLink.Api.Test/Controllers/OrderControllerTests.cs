using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Requests;
using BurgerLink.Order.Contracts.Responses;

namespace BurgerLink.Api.Test.Controllers;

public class OrderControllerTests
{
    [Fact]
    public async Task It_Should_Add_An_Item_To_An_Order()
    {
        await MassTransitTestHarness.RunTest(Test, Configurator);
        return;

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<SagaModifyOrderAddItem>(context => context.RespondAsync(new OrderUpdateAccepted()));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/order/addItem";
            var httpResponseMessage = await client.PostAsync(urlGetAll, JsonContent.Create(new SagaModifyOrderAddItem
            {
                OrderName = "Test",
                ItemName = "Test Item"
            }));

            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.Equal(StatusCodes.Status202Accepted, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Consumed.Any<SagaModifyOrderAddItem>());
        }
    }

    [Fact]
    public async Task It_Should_Get_A_404_When_Order_Does_Not_Exist()
    {
        const string orderName = "test_name";

        await MassTransitTestHarness.RunTest(Test, Configurator);
        return;

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<SagaOrderStatusRequest>(context => context.RespondAsync(new OrderNotFound
            {
                OrderName = context.Message.OrderName
            }));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = $"/order?orderName={orderName}";
            var httpResponseMessage = await client.GetAsync(urlGetAll);
            var orderStatus = await httpResponseMessage.Content.ReadFromJsonAsync<OrderNotFound>();

            Assert.NotNull(orderStatus);
            Assert.EndsWith(orderStatus.OrderName, orderName);
            Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Consumed.Any<SagaOrderStatusRequest>());
        }
    }


    [Fact]
    public async Task It_Should_Request_An_Orders_Status()
    {
        const string orderName = "test_name";

        await MassTransitTestHarness.RunTest(Test, Configurator);
        return;

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<SagaOrderStatusRequest>(context => context.RespondAsync(new OrderStatus
            {
                OrderName = context.Message.OrderName
            }));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = $"/order?orderName={orderName}";
            var httpResponseMessage = await client.GetAsync(urlGetAll);
            var orderStatus = await httpResponseMessage.Content.ReadFromJsonAsync<OrderStatus>();

            httpResponseMessage.EnsureSuccessStatusCode();

            Assert.NotNull(orderStatus);
            Assert.EndsWith(orderStatus.OrderName, orderName);
            Assert.Equal(StatusCodes.Status200OK, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Consumed.Any<SagaOrderStatusRequest>());
        }
    }


    [Fact]
    public async Task It_Should_Return_404_If_Order_Not_Found()
    {
        await MassTransitTestHarness.RunTest(Test, Configurator);
        return;

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<SagaModifyOrderAddItem>(context => context.RespondAsync(new OrderNotFound
            {
                OrderName = context.Message.OrderName,
                Timestamp = DateTime.UtcNow
            }));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/order/addItem";
            var httpResponseMessage = await client.PostAsync(urlGetAll, JsonContent.Create(new SagaModifyOrderAddItem
            {
                OrderName = "Test",
                ItemName = "Test Item"
            }));

            Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Consumed.Any<SagaModifyOrderAddItem>());
        }
    }

    [Fact]
    public async Task It_Should_Start_An_Order()
    {
        await MassTransitTestHarness.RunTest(Test);
        return;

        async Task Test(ITestHarness harness, HttpClient client)
        {
            var orderId = NewId.NextGuid();
            const string urlGetAll = "/order";
            var httpResponseMessage = await client.PostAsync(urlGetAll, JsonContent.Create(new SagaCreateOrder
            {
                OrderId = orderId,
                OrderName = "Test"
            }));

            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.Equal(StatusCodes.Status202Accepted, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Published.Any<SagaCreateOrder>());
        }
    }

    [Fact]
    public async Task It_Should_Start_Order_Preparation()
    {
        await MassTransitTestHarness.RunTest(Test);
        return;

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/order/start";
            var httpResponseMessage = await client.PostAsync(urlGetAll, JsonContent.Create(new SagaBeginPreparation
            {
                OrderName = "Test"
            }));

            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.Equal(StatusCodes.Status202Accepted, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Published.Any<SagaBeginPreparation>());
        }
    }
}