using Application.Common.Interfaces;
using Application.Entities.Bills.EventHandlers;
using Contracts;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Alcohols.EventHandlers
{
    public class AlcoholCreatedEventHandler : INotificationHandler<AlcoholCreatedEvent>
    {
        private readonly ILogger<AlcoholCreatedEventHandler> _logger;
        private readonly IBusBroker _bus;

        public AlcoholCreatedEventHandler(ILogger<AlcoholCreatedEventHandler> logger, IBusBroker bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(AlcoholCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventMessage = new AlcoholCreatedMessage
            {
                Id = notification.Ord.Id,
                CreatedOnUtc = DateTime.UtcNow,
            };

            await _bus.Publish(eventMessage, cancellationToken);

            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        }
    }
}
