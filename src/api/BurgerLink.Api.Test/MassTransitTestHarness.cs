using Microsoft.AspNetCore.Mvc.Testing;

namespace BurgerLink.Api.Test;

public static class MassTransitTestHarness
{
    private static void CopySettingsFile()
    {
        var current = Path.Combine(Directory.GetCurrentDirectory(), "settings");

        if (!Directory.Exists(current))
        {
            Directory.CreateDirectory(current);
        }

        const string sourceFileName = "commonsettings.json";
        var filePath = Path.Combine(current, sourceFileName);
        if (!File.Exists(filePath))
        {
            File.Copy(sourceFileName, filePath);
        }
    }

    public static async Task RunTest(
        Func<ITestHarness, HttpClient, Task> test,
        Action<IBusRegistrationConfigurator>? configurator = null)
    {
        CopySettingsFile();

        await using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    if (configurator is null)
                    {
                        services.AddMassTransitTestHarness();
                    }
                    else
                    {
                        services.AddMassTransitTestHarness(configurator);
                    }
                });
            });

        var harness = application.Services.GetTestHarness();
        await harness.Start();

        using var client = application.CreateClient();

        try
        {
            await test(harness, client);
        }
        finally
        {
            await harness.Stop();
        }
    }
}