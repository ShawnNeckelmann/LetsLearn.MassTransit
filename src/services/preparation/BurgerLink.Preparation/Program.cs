using System.Reflection;
using BurgerLink.Shared.AppConfiguration;
using MassTransit;

namespace BurgerLink.Preparation;

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
                        configurator.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                        configurator.ConfigureEndpoints(context);
                    });
                });

                services.AddMassTransitHostedService(true);
            });
    }
}