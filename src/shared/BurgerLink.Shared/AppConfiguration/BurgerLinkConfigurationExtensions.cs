using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace BurgerLink.Shared.AppConfiguration;

public static class BurgerLinkConfigurationExtensions
{
    public static void LoadAppSettings(this ConfigurationManager builder)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "settings", "commonsettings.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("commonsettings.json not found.", path);
        }

        builder.Sources.Clear();
        builder
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile(path, false, true)
            .AddEnvironmentVariables();
    }
}