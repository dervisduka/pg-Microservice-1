using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AlcoholUpdateDto : IMapFrom<Alcohol>
    {
        public Decimal Amount { get; set; }
        public int Status { get; set; }

        public Decimal AlcoholPercentage { get; set; }

        public Decimal PricePerBottle { get; set; }
    }
}
