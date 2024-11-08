using Application.Common.Mappings;
using Domain.Entities;

namespace Application.DTO
{
    public class AlcoholBriefDto : IMapFrom<Alcohol>
    {
        public int Id { get; set; }
        public Decimal Amount { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
