using BurgerLink.Ui.Repository.Inventory;
using BurgerLink.Ui.Repository.Inventory.Models;
using BurgerLink.Ui.Repository.Orders;
using BurgerLink.Ui.Repository.Orders.Models;

namespace BurgerLink.Ui.Infrastructure;

public static class ExtensionConfigureDependencyInjection
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddHostedService<BurgerLinkEventHubNotifier>();
        services.AddTransient<IInventoryRepository, InventoryMongoDbRepository>();
        services.AddTransient<IOrdersRepository, OrderMongoDbRepository>();

        services.Configure<InventorySettings>(configurationManager.GetRequiredSection("UiDatabase").GetRequiredSection("InventoryCollection"));
        services.Configure<OrderSettings>(configurationManager.GetRequiredSection("UiDatabase").GetRequiredSection("OrderCollection"));
        return services;
    }
}