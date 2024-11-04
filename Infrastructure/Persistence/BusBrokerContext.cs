using Application.Common.Interfaces;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class BusBrokerContext : IBusBroker
    {
        private readonly IBus _bus;
        private readonly ILogger<BusBrokerContext> _logger;
        public BusBrokerContext(IBus bus, ILogger<BusBrokerContext> logger)
        {
            _bus = bus;
            _logger = logger;
        }
        public async Task Publish(object eventMessage, CancellationToken cancellationToken)
        {
            try
            {
                await _bus.Publish(eventMessage, cancellationToken);
                _logger.LogInformation("Succesfully published message: {eventMessage}", eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "An exception was thron while publishing the message: {eventMessage}", eventMessage);
                throw;
            }

        }
    }
}
