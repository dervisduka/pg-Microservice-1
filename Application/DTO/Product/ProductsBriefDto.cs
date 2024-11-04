using Application.Common.Mappings;
using Domain.Entities;

namespace Application.DTO
{
    public class ProductsBriefDto : IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }

        public string Rate { get; set; }
        public string AddedOn { get; set; }
    }
}
