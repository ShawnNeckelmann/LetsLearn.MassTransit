using BurgerLink.Shared.AppConfiguration;
using BurgerLink.Ui.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();

var cs = builder.Configuration.GetConnectionString("RabbitMq") ??
         throw new ArgumentNullException("builder.Configuration.GetConnectionString(\"RabbitMq\")");

var otelAddress = builder.Configuration.GetSection("Otel")["Address"] ??
                  throw new ArgumentNullException("builder.Configuration.GetSection(\"Otel\")[\"Address\"]");

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSignalR();
builder.Logging.ConfigureLogging(otelAddress);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder
        .WithOrigins("https://localhost:44419")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services
    .AddHttpLogging(options => { options.LoggingFields = HttpLoggingFields.All; })
    .ConfigureDependencyInjection(builder.Configuration)
    .AddAndConfigureMassTransit(cs)
    .ConfigureTelemetry(otelAddress);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.MapHub<BurgerLinkEventHub>("/events");
app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();