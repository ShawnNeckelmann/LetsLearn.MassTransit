using BurgerLink.Shared.AppConfiguration;

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
            .ConfigureAppConfiguration((_, builder) => BurgerLinkConfigurationExtensions.LoadAppSettings(builder))
            .ConfigureLogging()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddAndConfigureMassTransit(hostContext.Configuration.GetConnectionString("RabbitMq"));
            });
    }
}