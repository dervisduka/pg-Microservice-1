using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Bills.EventHandlers
{
    public class BillCreatedEventHandler : INotificationHandler<BillCreatedEvent>
    {
        private readonly ILogger<BillCreatedEventHandler> _logger;

        public BillCreatedEventHandler(ILogger<BillCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BillCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
