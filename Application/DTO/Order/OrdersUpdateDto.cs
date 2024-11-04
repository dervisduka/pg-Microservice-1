﻿using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class OrdersUpdateDto : IMapFrom<Product>
    {
        public Decimal Amount { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
    }
}
