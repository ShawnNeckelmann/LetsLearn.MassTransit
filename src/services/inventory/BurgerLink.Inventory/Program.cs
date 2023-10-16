using BurgerLink.Inventory.Services;
using BurgerLink.Shared.AppConfiguration;
using BurgerLink.Shared.MongDbConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();
builder.Host.ConfigureLogging();

// Add services to the container.
builder.Services
    .ConfigureIt(builder.Configuration)
    .AddAndConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry();

var app = builder.Build();

app.Run();

internal static class ConfiguratgionTextension
{
    internal static IServiceCollection ConfigureIt(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IInventoryService, MongoDbInventoryService>();
        services.Configure<MongoDbSettings>(configuration.GetSection("InventoryDatabase"));

        return services;
    }
}