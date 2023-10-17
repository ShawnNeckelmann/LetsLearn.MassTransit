using BurgerLink.Shared.AppConfiguration;

// This must be set before creating a GrpcChannel/HttpClient when calling an insecure service
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();
builder.Host.ConfigureLogging();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAndConfigureMassTransit(builder.Configuration.GetConnectionString("RabbitMq"))
    .ConfigureTelemetry();

var app = builder.Build();

app.UseHttpLogging();

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