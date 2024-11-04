using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Products.Queries
{
    public record GetAllProductsEmptyCriteria() : IRequest<IReadOnlyList<Product>>;


    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsEmptyCriteria, IReadOnlyList<Product>>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Product>> Handle(GetAllProductsEmptyCriteria criteria, CancellationToken cancellationToken)
        {

            var data = await _unit.Products.GetAllAsync();

            //bejme return versionin e mapuar
            return data;
        }
    }
}
