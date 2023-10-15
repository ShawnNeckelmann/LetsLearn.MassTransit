using System.Reflection;
using MassTransit.Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

        serviceCollection.AddOpenTelemetry()
            .ConfigureResource(o => ConfigureResource(o, serviceName))
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource(DiagnosticHeaders.DefaultListenerName)
                    .AddZipkinExporter(zipkinExporterOptions =>
                    {
                        var hostName = new Uri("http://zipkin:9411/api/v2/spans");
                        zipkinExporterOptions.Endpoint = hostName;
                    });
            });
    }
}