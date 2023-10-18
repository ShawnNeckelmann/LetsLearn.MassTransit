using System.Reflection;
using BurgerLink.Order.Consumers.PreparationComplete;
using BurgerLink.Order.Contracts;
using BurgerLink.Order.Contracts.Commands;
using BurgerLink.Order.Contracts.Requests;
using BurgerLink.Order.Services;
using BurgerLink.Order.State;
using BurgerLink.Preparation.Contracts.Commands;
using BurgerLink.Shared.MongDbConfiguration;
using MassTransit;
using Microsoft.Extensions.Options;

internal static class ConfigurationExtension
{
    internal static IServiceCollection ConfigureMassTransit(this IServiceCollection services, string csRabbitMq)
    {
        return services.AddMassTransit(configurator =>
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.AddConsumers(entryAssembly);
            configurator.AddActivities(entryAssembly);

            configurator.UsingRabbitMq((context, mqBusFactoryConfigurator) =>
            {
                mqBusFactoryConfigurator.Host(csRabbitMq);
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
    }

    internal static IServiceCollection ConfigureServiceCollection(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddSingleton<IOrderService, OrderService>();
        services.Configure<MongoDbSettings>(configurationManager.GetSection("OrderDatabase"));
        return services;
    }
}