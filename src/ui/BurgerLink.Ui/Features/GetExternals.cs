using MediatR;

namespace BurgerLink.Ui.Features;

public class GetExternals
{
    public record External
    {
        public required string Address { get; init; }
        public required string Credentials { get; init; }

        public required string Information { get; init; }

        public required string Name { get; init; }
    }

    public class GetExternalsRequest : IRequest<GetExternalsResponse>
    {
    }

    public class GetExternalsRequestHandler : IRequestHandler<GetExternalsRequest, GetExternalsResponse>
    {
        public async Task<GetExternalsResponse> Handle(GetExternalsRequest request, CancellationToken cancellationToken)
        {
            var externals = new List<External>
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
            var retval = new GetExternalsResponse
            {
                Externals = externals
            };

            return await Task.FromResult(retval);
        }
    }

    public class GetExternalsResponse
    {
        public List<External> Externals { get; set; }
    }
}