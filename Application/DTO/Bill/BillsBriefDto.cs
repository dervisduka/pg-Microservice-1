using Application.Common.Mappings;
using Domain.Entities;

namespace Application.DTO
{
    public class BillsBriefDto : IMapFrom<Bill>
    {
        public int Id { get; set; }
        public Decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
