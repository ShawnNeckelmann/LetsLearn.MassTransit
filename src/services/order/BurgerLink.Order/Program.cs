using System.Reflection;
using BurgerLink.Order.Services;
using BurgerLink.Order.State;
using BurgerLink.Shared.AppConfiguration;
using BurgerLink.Shared.MongDbConfiguration;
using MassTransit;
using Microsoft.Extensions.Options;

namespace BurgerLink.Order;

public class Program
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
                services.AddSingleton<IOrderService, OrderService>();
                services.Configure<MongoDbSettings>(hostContext.Configuration.GetSection("OrderDatabase"));

                services.AddMassTransit(configurator =>
                {
                    var entryAssembly = Assembly.GetEntryAssembly();

                    configurator.SetKebabCaseEndpointNameFormatter();

                    configurator.AddConsumers(entryAssembly);
                    configurator.AddActivities(entryAssembly);

                    configurator.UsingRabbitMq((context, mqBusFactoryConfigurator) =>
                    {
                        mqBusFactoryConfigurator.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                        mqBusFactoryConfigurator.ReceiveEndpoint("saga-order",
                            endpointConfigurator =>
                            {
                                endpointConfigurator.ConfigureSaga<OrderState>(context, x => { });
                            });
                        mqBusFactoryConfigurator.ConfigureEndpoints(context);
                    });

                    configurator
                        .AddSagaStateMachine<OrderStateMachine, OrderState>()
                        .MongoDbRepository(mongoDbSagaRepositoryConfigurator =>
                        {
                            var sp = services.BuildServiceProvider();
                            var options = sp.GetRequiredService<IOptions<MongoDbSettings>>();
                            var settings = options.Value;

                            mongoDbSagaRepositoryConfigurator.Connection = settings.ConnectionString;
                            mongoDbSagaRepositoryConfigurator.DatabaseName = settings.DatabaseName;
                            mongoDbSagaRepositoryConfigurator.CollectionName = settings.CollectionName;
                        });
                });

                services.AddMassTransitHostedService(true);
            });
    }
}