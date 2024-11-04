using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Products.Queries
{
    public record ProductsWithPaginationCriteria : IRequest<PaginatedList<ProductsBriefDto>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool UseNameWithLike { get; set; } = false;
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public bool IsCreatedDateBetweenUsed { get; set; } = false;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

    public class GetProductsWithPaginationQueryHandler : IRequestHandler<ProductsWithPaginationCriteria, PaginatedList<ProductsBriefDto>>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public GetProductsWithPaginationQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProductsBriefDto>> Handle(ProductsWithPaginationCriteria criteria, CancellationToken cancellationToken)
        {

            //ketu kthehet PaginatedList<Product>
            PaginatedList<Product> Lista = await _unit.Products.GetProductsByCriteria(criteria);

            //bejme return versionin e mapuar
            return new PaginatedList<ProductsBriefDto>(_mapper.Map<List<Product>, List<ProductsBriefDto>>(Lista.Items), Lista.TotalCount, criteria.PageNumber, criteria.PageSize);
        }
    }
}
