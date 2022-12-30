using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureAppConfiguration((_, builder1) => BurgerLinkConfigurationExtensions.LoadAppSettings(builder1))
    .ConfigureLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAndConfigureMassTransit(
    builder.Configuration.GetConnectionString("RabbitMq")
);

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.MapControllers();

app.Run();