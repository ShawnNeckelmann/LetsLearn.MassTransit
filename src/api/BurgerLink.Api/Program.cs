using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.LoadAppSettings();

var connectionStringRabbitMq = builder.Configuration.GetConnectionString("RabbitMq") ??
                               throw new ArgumentNullException(
                                   "builder.Configuration.GetConnectionString(\"RabbitMq\")");

var otelAddress = builder.Configuration.GetSection("Otel")["Address"] ??
                  throw new ArgumentNullException("builder.Configuration.GetSection(\"Otel\")[\"Address\"]");


builder.Logging.ConfigureLogging(otelAddress);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAndConfigureMassTransit(connectionStringRabbitMq)
    .ConfigureTelemetry(otelAddress);

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