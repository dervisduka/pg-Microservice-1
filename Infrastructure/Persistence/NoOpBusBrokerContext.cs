using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class NoOpBusBrokerContext : IBusBroker
    {
        public Task Publish(object eventMessage, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
