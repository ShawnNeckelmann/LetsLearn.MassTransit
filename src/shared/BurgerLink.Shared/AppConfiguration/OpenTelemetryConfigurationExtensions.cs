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

    private static Action<OtlpExporterOptions> ConfigureExporter(string httpOtelCollector)
    {
        return otlpExporterOptions =>
        {
            otlpExporterOptions.ExportProcessorType = ExportProcessorType.Simple;
            otlpExporterOptions.Protocol = OtlpExportProtocol.Grpc;
            otlpExporterOptions.Endpoint = new Uri(httpOtelCollector);
        };
    }

    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder, string httpOtelCollector)
    {
        loggingBuilder
            .ClearProviders()
            .AddConsole()
            .AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;

                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);


                options
                    .SetResourceBuilder(ResourceBuilder())
                    //.AddConsoleExporter()
                    .AddOtlpExporter(ConfigureExporter(httpOtelCollector));

                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.ParseStateValues = true;
            });

        return loggingBuilder;
    }

    public static void ConfigureTelemetry(this IServiceCollection serviceCollection, string httpOtelCollector)
    {
        serviceCollection.AddOpenTelemetry()
            .ConfigureResource(builder => { builder.AddService(ServiceName); })
            .PrivateConfigureTracing(httpOtelCollector)
            .PrivateConfigureMetrics(httpOtelCollector);
    }

    private static OpenTelemetryBuilder PrivateConfigureMetrics(this OpenTelemetryBuilder openTelemetryBuilder,
        string httpOtelCollector)
    {
        return openTelemetryBuilder.WithMetrics(opts => opts
            .SetResourceBuilder(ResourceBuilder())
            .AddMeter(InstrumentationOptions.MeterName)
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            //.AddConsoleExporter()
            .AddOtlpExporter(ConfigureExporter(httpOtelCollector)));
    }

    private static OpenTelemetryBuilder PrivateConfigureTracing(this OpenTelemetryBuilder openTelemetryBuilder,
        string httpOtelCollector)
    {
        return openTelemetryBuilder.WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder())
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                //.AddConsoleExporter()
                .AddOtlpExporter(ConfigureExporter(httpOtelCollector));
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