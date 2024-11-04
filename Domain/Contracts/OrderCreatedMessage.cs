using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public record OrderCreatedMessage
    {
        public int Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }
    }
}
