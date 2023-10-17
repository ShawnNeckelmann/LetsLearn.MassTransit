using System.Reflection;
using MassTransit.Logging;
using MassTransit.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BurgerLink.Shared.AppConfiguration;

public static class OpenTelemetryConfigurationExtensions
{
    public static void ConfigureTelemetry(this IServiceCollection serviceCollection)
    {
        var serviceName = Assembly.GetEntryAssembly().GetName().Name;
        if (serviceName is null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        serviceCollection.AddOpenTelemetry()
            .ConfigureResource(builder => { builder.AddService(serviceName); })
            .PrivateConfigureTracing()
            .PrivateConfigureMetrics(serviceName);
    }

    private static OpenTelemetryBuilder PrivateConfigureMetrics(this OpenTelemetryBuilder openTelemetryBuilder,
        string serviceName)
    {
        return openTelemetryBuilder.WithMetrics(opts => opts
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

    private static OpenTelemetryBuilder PrivateConfigureTracing(this OpenTelemetryBuilder openTelemetryBuilder)
    {
        return openTelemetryBuilder.WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .AddConsoleExporter()
                .AddOtlpExporter(otlpExporterOptions =>
                {
                    otlpExporterOptions.Endpoint = new Uri("http://otel-collector:4317/");
                });
        });
    }
}