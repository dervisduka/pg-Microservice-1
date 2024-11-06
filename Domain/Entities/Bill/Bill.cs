using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Bill : BaseAuditableEntityConcurrent
    {
        [Column(TypeName = "decimal(18,4)")]
        public Decimal Amount { get; set; }
        public int Status { get; set; }
    }
}
