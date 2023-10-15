using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();

builder.Host
    .ConfigureLogging();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAndConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry("BurgerLink.Api");

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.MapControllers();

app.Run();


namespace BurgerLink.Api
{
    public class Program
    {
    }
}