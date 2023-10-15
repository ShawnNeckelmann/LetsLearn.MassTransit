using System.Reflection;
using BurgerLink.Order.Consumers.PreparationComplete;
using BurgerLink.Order.Contracts;
using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Requests;
using BurgerLink.Order.Services;
using BurgerLink.Order.State;
using BurgerLink.Preparation.Contracts.Commands;
using BurgerLink.Shared.AppConfiguration;
using BurgerLink.Shared.MongDbConfiguration;
using MassTransit;
using Microsoft.Extensions.Options;

namespace BurgerLink.Order;

public class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, builder) => builder.LoadAppSettings())
            .ConfigureLogging()
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureTelemetry();
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
                                endpointConfigurator.ConfigureSaga<OrderState>(context, sagaConfigurator =>
                                {
                                    var partition = sagaConfigurator.CreatePartitioner(5);

                                    sagaConfigurator.Message<SagaModifyOrderAddItem>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<SagaBeginPreparation>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<SagaCreateOrder>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<ItemPrepared>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<ItemUnavailable>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<ItemAvailabilityValidated>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<PreparationComplete>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                    sagaConfigurator.Message<SagaOrderStatusRequest>(x =>
                                        x.UsePartitioner(partition,
                                            consumeContext => consumeContext.Message.OrderName));
                                });
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
            });
    }

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }
}