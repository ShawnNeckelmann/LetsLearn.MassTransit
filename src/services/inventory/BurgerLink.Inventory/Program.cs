using System.Reflection;
using BurgerLink.Inventory.Services;
using BurgerLink.Shared.AppConfiguration;
using BurgerLink.Shared.MongDbConfiguration;
using MassTransit;

namespace BurgerLink.Inventory;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(BurgerLinkConfigurationExtensions.LoadAppSettings)
            .ConfigureLogging()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IInventoryService, MongoDbInventoryService>();

                services.Configure<MongoDbSettings>(hostContext.Configuration.GetSection("InventoryDatabase"));

                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    var entryAssembly = Assembly.GetEntryAssembly();

                    x.AddConsumers(entryAssembly);
                    x.AddSagaStateMachines(entryAssembly);
                    x.AddSagas(entryAssembly);
                    x.AddActivities(entryAssembly);
                    x.UsingRabbitMq((context, configurator) =>
                    {
                        var cs = hostContext.Configuration.GetConnectionString("RabbitMq");
                        configurator.Host(cs);
                        configurator.ConfigureEndpoints(context);
                    });
                });

                services.AddMassTransitHostedService(true);
            });
    }
}