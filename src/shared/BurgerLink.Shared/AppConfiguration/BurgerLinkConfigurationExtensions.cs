using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace BurgerLink.Shared.AppConfiguration;

public static class BurgerLinkConfigurationExtensions
{
    public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((host, log) =>
        {
            var applicationName = Assembly.GetEntryAssembly().GetName().Name;

            log.MinimumLevel.Debug();
            log.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            log.MinimumLevel.Override("Quartz", LogEventLevel.Information);
            log.WriteTo.Console();
            log.WriteTo.GrafanaLoki("http://loki:3100", new List<LokiLabel>
            {
                new()
                {
                    Key = "Application",
                    Value = applicationName
                },
                new()
                {
                    Key = "Environment",
                    Value = host.HostingEnvironment.EnvironmentName
                }
            });
        });
    }

    public static void LoadAppSettings(IConfigurationBuilder builder)
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

        builder.Build();
    }
}