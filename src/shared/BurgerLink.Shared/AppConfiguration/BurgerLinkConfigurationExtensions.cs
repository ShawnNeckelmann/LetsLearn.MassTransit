using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
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

    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder, string environment)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(Assembly.GetEntryAssembly().GetName().Name, serviceVersion: "1.0.0")
            .AddTelemetrySdk()
            .AddAttributes(new Dictionary<string, object>
            {
                ["host.name"] = Environment.MachineName,
                ["deployment.environment"] = environment
            });

        return loggingBuilder
            .ClearProviders()
            .AddOpenTelemetry(options =>
            {
                options
                    .SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter()
                    .AddConsoleExporter();

                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.ParseStateValues = true;
            });
    }

    public static void LoadAppSettings(this IConfigurationBuilder builder)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "settings", "commonsettings.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("commonsettings.json not found.", path);
        }

        builder.Sources.Clear();
        builder
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile(path, false, true);

        builder.Build();
    }


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