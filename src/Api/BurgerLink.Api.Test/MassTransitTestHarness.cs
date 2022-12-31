using Microsoft.AspNetCore.Mvc.Testing;

namespace BurgerLink.Api.Test;

public static class MassTransitTestHarness
{
    public static async Task RunTest(
        Func<ITestHarness, HttpClient, Task> test,
        Action<IBusRegistrationConfigurator>? configurator = null)
    {
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