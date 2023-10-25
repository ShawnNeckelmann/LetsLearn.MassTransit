using BurgerLink.Inventory.Services;
using BurgerLink.Shared.MongDbConfiguration;

internal static class ConfigurationExtension
{
    internal static IServiceCollection ConfigureServiceCollection(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IInventoryService, MongoDbInventoryService>();
        services.Configure<MongoDbSettings>(configuration.GetSection("InventoryDatabase"));

        return services;
    }
}