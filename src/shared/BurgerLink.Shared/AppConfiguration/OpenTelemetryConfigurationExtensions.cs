using System.Reflection;
using System.Runtime.InteropServices;
using MassTransit.Logging;
using MassTransit.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BurgerLink.Shared.AppConfiguration;

public static class OpenTelemetryConfigurationExtensions
{
    private static string ServiceName
    {
        get
        {
            var serviceName = Assembly.GetEntryAssembly()?.GetName().Name;
            return string.IsNullOrEmpty(serviceName) ? throw new ArgumentNullException() : serviceName;
        }
    }

    private static Action<OtlpExporterOptions> ConfigureExporter()
    {
        return otlpExporterOptions =>
        {
            otlpExporterOptions.ExportProcessorType = ExportProcessorType.Simple;
            otlpExporterOptions.Protocol = OtlpExportProtocol.Grpc;
            otlpExporterOptions.Endpoint = new Uri("http://otel-collector:4317/");
        };
    }

    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder
            .ClearProviders()
            .AddOpenTelemetry(options =>
            {
                options
                    .SetResourceBuilder(ResourceBuilder())
                    .AddConsoleExporter()
                    .AddOtlpExporter(ConfigureExporter());

                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.ParseStateValues = true;
            });

        return loggingBuilder;
    }

    public static void ConfigureTelemetry(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOpenTelemetry()
            .ConfigureResource(builder => { builder.AddService(ServiceName); })
            .PrivateConfigureTracing()
            .PrivateConfigureMetrics();
    }

    private static OpenTelemetryBuilder PrivateConfigureMetrics(this OpenTelemetryBuilder openTelemetryBuilder)
    {
        return openTelemetryBuilder.WithMetrics(opts => opts
            .SetResourceBuilder(ResourceBuilder())
            .AddMeter(InstrumentationOptions.MeterName)
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(ConfigureExporter()));
    }

    private static OpenTelemetryBuilder PrivateConfigureTracing(this OpenTelemetryBuilder openTelemetryBuilder)
    {
        return openTelemetryBuilder.WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder())
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .AddConsoleExporter()
                .AddOtlpExporter(ConfigureExporter());
        });
    }

    private static ResourceBuilder ResourceBuilder()
    {
        return OpenTelemetry.Resources.ResourceBuilder
            .CreateDefault()
            .AddService(ServiceName)
            .AddTelemetrySdk()
            .AddAttributes(new Dictionary<string, object>
            {
                ["host.name"] = Environment.MachineName,
                ["os.description"] = RuntimeInformation.OSDescription
            });
    }
}