using BurgerLink.Shared.MongDbConfiguration;
using BurgerLink.Ui.Repository.Inventory;
using BurgerLink.Ui.Repository.Orders;

namespace BurgerLink.Ui.Infrastructure;

public static class ExtensionConfigureDependencyInjection
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddHostedService<BurgerLinkEventHubNotifier>();
        services.AddSingleton<IInventoryRepository, InventoryMongoDbRepository>();
        services.AddSingleton<IOrdersRepository, OrderMongoDbRepository>();
        services.Configure<MongoDbSettings>(configurationManager.GetSection("UiDatabase"));
        return services;
    }
}