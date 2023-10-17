using System.Reflection;
using MassTransit.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace BurgerLink.Shared.AppConfiguration;

public static class OpenTelemetryConfigurationExtensions
{
    private static void ConfigureResource(ResourceBuilder obj, string serviceName)
    {
        obj.AddService(serviceName, serviceInstanceId: Environment.MachineName);
    }

    public static void ConfigureTelemetry(this IServiceCollection serviceCollection)
    {
        var serviceName = Assembly.GetEntryAssembly().GetName().Name;

        serviceCollection.AddOpenTelemetry().WithMetrics(opts => opts
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddMeter(InstrumentationOptions.MeterName)
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(otlpExporterOptions =>
            {
                otlpExporterOptions.Endpoint = new Uri("http://otel-collector:4317/");
            }));
    }
}