using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogging(builder.Environment.EnvironmentName);
builder.Configuration.LoadAppSettings();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAndConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry();

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