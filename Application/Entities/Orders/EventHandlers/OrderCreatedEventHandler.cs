using Application.Common.Interfaces;
using Contracts;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Orders.EventHandlers
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;
        private readonly IBusBroker _bus;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, IBusBroker bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventMessage = new OrderCreatedMessage
            {
                Id = notification.Ord.Id,
                CreatedOnUtc = DateTime.UtcNow,
            };

            await _bus.Publish(eventMessage, cancellationToken);

            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        }
    }
}
