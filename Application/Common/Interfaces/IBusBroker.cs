﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IBusBroker
    {
        Task Publish(object eventMessage, CancellationToken cancellationToken);
    }
}
