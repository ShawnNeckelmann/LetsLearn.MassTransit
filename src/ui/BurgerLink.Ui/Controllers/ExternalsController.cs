using BurgerLink.Ui.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Ui.Controllers;

public class ExternalsController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        var retval = new List<External>
        {
            new()
            {
                Address = "http://localhost:15672/#/",
                Credentials = @"User: guest // Password: guest",
                Information = "Managmeent for the Rabbit Cluster",
                Name = "RabbitMq Management"
            },
            new()
            {
                Name = "Mongo Express",
                Address = "http://localhost:8081",
                Information = "Managmeent for the Mongo Server",
                Credentials = "User: admin // Password: pass"
            },
            new()
            {
                Name = "Grafana",
                Address = "http://localhost:3000/",
                Information = "Metrics, traces, and logs.",
                Credentials = ""
            }
        };


        return Ok(retval);
    }
}

public record External
{
    public required string Address { get; init; }
    public required string Credentials { get; init; }

    public required string Information { get; init; }

    public required string Name { get; init; }
}