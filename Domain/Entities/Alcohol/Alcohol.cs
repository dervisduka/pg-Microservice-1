﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Alcohol : BaseEntityConcurrent
    {
        [Column(TypeName = "decimal(18,4)")]
        public Decimal Amount { get; set; }
        public int Status { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public Decimal AlcoholPercentage { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public Decimal PricePerBottle { get; set; }
    }
}
