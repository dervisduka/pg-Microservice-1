﻿using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Products.EventHandlers
{
    public class ProductCreatedEventHandler1 : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedEventHandler1> _logger;

        public ProductCreatedEventHandler1(ILogger<ProductCreatedEventHandler1> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
