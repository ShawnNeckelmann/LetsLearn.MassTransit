﻿using BurgerLink.Shared.MongDbConfiguration;
using BurgerLink.Ui.Repository.Inventory;

namespace BurgerLink.Ui.Infrastructure;

public static class ExtensionConfigureDependencyInjection
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddHostedService<BurgerLinkEventHubNotifier>();
        services.AddSingleton<IInventoryRepository, InventoryMongoDbRepository>();
        services.Configure<MongoDbSettings>(configurationManager.GetSection("UiDatabase"));
        return services;
    }
}