using BurgerLink.Inventory.Contracts.Commands;
using BurgerLink.Inventory.Contracts.Requests;
using BurgerLink.Inventory.Contracts.Responses;

namespace BurgerLink.Api.Test.Controllers;

public class InventoryControllerTests
{
    [Fact]
    public async Task It_Should_Request_AllItems()
    {
        var responseAllItems = new AllInventoryItems
        {
            Count = 1,
            Items = new List<InventoryItem>
            {
                new()
                {
                    ItemName = "test1",
                    Quantity = 1
                }
            }
        };

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<GetAllInventoryItems>(context => context.RespondAsync(responseAllItems));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/inventory/all";
            var httpResponseMessage = await client.GetAsync(urlGetAll);


            httpResponseMessage.EnsureSuccessStatusCode();

            Assert.True(await harness.Consumed.Any<GetAllInventoryItems>());

            var allItems = await httpResponseMessage.Content.ReadFromJsonAsync<AllInventoryItems>();
            Assert.NotNull(allItems);
            Assert.Equal(responseAllItems.Count, allItems.Count);
        }

        await MassTransitTestHarness.RunTest(
            Test,
            Configurator
        );
    }

    [Fact]
    public async Task It_Should_Request_OneItem()
    {
        var inventoryItem = new InventoryItem
        {
            Quantity = 1,
            ItemName = "TestItem1"
        };

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<GetInventoryItem>(context => context.RespondAsync(inventoryItem));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/inventory?itemName=TestItem1";
            var httpResponseMessage = await client.GetAsync(urlGetAll);


            httpResponseMessage.EnsureSuccessStatusCode();

            Assert.True(await harness.Consumed.Any<GetInventoryItem>());

            var allItems = await httpResponseMessage.Content.ReadFromJsonAsync<InventoryItem>();
            Assert.NotNull(allItems);
            Assert.Equal(inventoryItem.Quantity, allItems.Quantity);
        }

        await MassTransitTestHarness.RunTest(
            Test,
            Configurator
        );
    }

    [Fact]
    public async Task It_Should_Return_404_If_Not_Found()
    {
        var notFound = new InventoryItemNotFound();

        void Configurator(IBusRegistrationConfigurator configurator)
        {
            configurator.AddHandler<GetInventoryItem>(context => context.RespondAsync(notFound));
        }

        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/inventory?itemName=TestItem1";

            var httpResponseMessage = await client.GetAsync(urlGetAll);

            Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Consumed.Any<GetInventoryItem>());
            var allItems = await httpResponseMessage.Content.ReadFromJsonAsync<InventoryItemNotFound>();
            Assert.NotNull(allItems);
        }

        await MassTransitTestHarness.RunTest(
            Test,
            Configurator
        );
    }

    [Fact]
    public async Task It_Should_Create_An_Item()
    {
        async Task Test(ITestHarness harness, HttpClient client)
        {
            const string urlGetAll = "/inventory";

            var httpResponseMessage = await client.PostAsync(urlGetAll, JsonContent.Create(new UpsertInventoryItem()
            {
                ItemName = "Test",
                Quantity = 1
            }));

            Assert.Equal(StatusCodes.Status202Accepted, (int)httpResponseMessage.StatusCode);
            Assert.True(await harness.Published.Any<UpsertInventoryItem>());
        }

        await MassTransitTestHarness.RunTest(Test);
    }
}