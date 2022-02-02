using BurgerLink.Shared.AppConfiguration;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .ConfigureAppConfiguration(BurgerLinkConfigurationExtensions.LoadAppSettings)
    .ConfigureLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGenericRequestClient();
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((hostContext, factoryConfigurator) =>
    {
        factoryConfigurator.Host(builder.Configuration.GetConnectionString("RabbitMq"));
        factoryConfigurator.ConfigureEndpoints(hostContext);
    });
}).AddMassTransitHostedService(true);

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();