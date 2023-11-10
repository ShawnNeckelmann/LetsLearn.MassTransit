using BurgerLink.Shared.AppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.LoadAppSettings();
builder.Logging.ConfigureLogging("http://otel-collector:4317/");

builder.Services.AddControllersWithViews();


var cs = builder.Configuration.GetConnectionString("RabbitMq") ??
         throw new ArgumentNullException("builder.Configuration.GetConnectionString(\"RabbitMq\")");

var otelAddress = builder.Configuration.GetSection("Otel")["Address"] ??
                  throw new ArgumentNullException("builder.Configuration.GetSection(\"Otel\")[\"Address\"]");
builder.Services
    .AddAndConfigureMassTransit(cs)
    .ConfigureTelemetry(otelAddress);


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();