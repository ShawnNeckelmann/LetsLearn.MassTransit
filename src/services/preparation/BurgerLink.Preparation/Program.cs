using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();
builder.Logging.ConfigureLogging();

builder.Services
    .AddAndConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry();

var app = builder.Build();

app.Run();