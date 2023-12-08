using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();


var connectionStringRabbitMq = builder.Configuration.GetConnectionString("RabbitMq") ??
                               throw new ArgumentNullException(
                                   "builder.Configuration.GetConnectionString(\"RabbitMq\")");

var otelAddress = builder.Configuration.GetSection("Otel")["Address"] ??
                  throw new ArgumentNullException("builder.Configuration.GetSection(\"Otel\")[\"Address\"]");

builder.Logging.ConfigureLogging(otelAddress);

// Add services to the container.
builder.Services
    .ConfigureServiceCollection(builder.Configuration)
    .AddAndConfigureMassTransit(connectionStringRabbitMq)
    .ConfigureTelemetry(otelAddress);

var app = builder.Build();

app.Run();