using MediatR;

namespace BurgerLink.Ui.Features;

public class CreateOrder
{
    public class Command : IRequest<Unit>
    {
        public string OrderId { get; set; }
        public List<string> OrderItemIds { get; set; }
        public string OrderName { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>

    {
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Created");
            return Task.FromResult(Unit.Value);
        }
    }
}