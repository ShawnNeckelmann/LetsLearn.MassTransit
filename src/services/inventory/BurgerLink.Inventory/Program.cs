using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();
builder.Logging.ConfigureLogging();

// Add services to the container.
builder.Services
    .ConfigureServiceCollection(builder.Configuration)
    .AddAndConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry();

var app = builder.Build();

app.Run();