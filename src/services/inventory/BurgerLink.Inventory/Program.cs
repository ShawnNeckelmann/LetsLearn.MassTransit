using BurgerLink.Inventory.Services;
using BurgerLink.Shared.AppConfiguration;
using BurgerLink.Shared.MongDbConfiguration;

namespace BurgerLink.Inventory;

public static class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, builder) => BurgerLinkConfigurationExtensions.LoadAppSettings(builder))
            .ConfigureLogging()
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureTelemetry("BurgerLink.Inventory");
                services.AddSingleton<IInventoryService, MongoDbInventoryService>();
                services.Configure<MongoDbSettings>(hostContext.Configuration.GetSection("InventoryDatabase"));
                services.AddAndConfigureMassTransit(hostContext.Configuration.GetConnectionString("RabbitMq"));
            });
    }

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }
}