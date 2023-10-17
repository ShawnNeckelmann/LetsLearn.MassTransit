using BurgerLink.Shared.AppConfiguration;

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

//var s_meter = new Meter("HatCo.HatStore", "1.0.0");
//var s_hatsSold = s_meter.CreateCounter<int>(
//    "hats-sold",
//    "Hats",
//    "The number of hats sold in our store");

//using var meterProvider = Sdk.CreateMeterProviderBuilder()
//    .AddMeter("HatCo.HatStore")
//    .AddPrometheusHttpListener(options => options.UriPrefixes = new[] { "http://localhost:9184/" })
//    .Build();

//var rand = Random.Shared;
//Console.WriteLine("Press any key to exit");
//while (true)
//{
//    //// Simulate hat selling transactions.
//    Thread.Sleep(rand.Next(100, 2500));
//    s_hatsSold.Add(rand.Next(0, 1000));
//}

app.Run();


namespace BurgerLink.Api
{
    public class Program
    {
    }
}