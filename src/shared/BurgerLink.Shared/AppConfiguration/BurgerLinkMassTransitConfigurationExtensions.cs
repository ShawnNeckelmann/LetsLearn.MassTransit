using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BurgerLink.Shared.AppConfiguration;

public static class BurgerLinkMassTransitConfigurationExtensions
{
    public static IServiceCollection AddAndConfigureMassTransit(this IServiceCollection serviceCollection,
        string connectionStringRabbitMq)
    {
        serviceCollection.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            var entryAssembly = Assembly.GetEntryAssembly();

            configurator.AddConsumers(entryAssembly);
            configurator.AddSagaStateMachines(entryAssembly);
            configurator.AddSagas(entryAssembly);
            configurator.AddActivities(entryAssembly);

            configurator.UsingRabbitMq((hostContext, factoryConfigurator) =>
            {
                factoryConfigurator.Host(connectionStringRabbitMq);
                factoryConfigurator.ConfigureEndpoints(hostContext);
            });
        });
        return serviceCollection;
    }
}