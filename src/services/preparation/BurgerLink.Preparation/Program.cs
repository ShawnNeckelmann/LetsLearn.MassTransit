using BurgerLink.Shared.AppConfiguration;

namespace BurgerLink.Preparation;

public class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, builder) => BurgerLinkConfigurationExtensions.LoadAppSettings(builder))
            .ConfigureLogging()
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureTelemetry("BurgerLink.Preparation");
                services.AddAndConfigureMassTransit(hostContext.Configuration.GetConnectionString("RabbitMq"));
            });
    }

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }
}