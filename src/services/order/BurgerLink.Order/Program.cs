using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();
builder.Host.ConfigureLogging();

// Add services to the container
builder.Services
    .ConfigureServiceCollection(builder.Configuration)
    .ConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry();

var app = builder.Build();

app.Run();