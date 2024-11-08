using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Alcohols.Queries
{
    public record AlcoholsWithPaginationCriteria : IRequest<PaginatedList<AlcoholBriefDto>>
    {
        public int? Id { get; set; }
        public Decimal? Amount { get; set; }
        public int? Status { get; set; }

        public Decimal? AlcoholPercentage { get; set; }

        public Decimal? PricePerBottle { get; set; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

    public class GetAlcoholsWithPaginationQueryHandler : IRequestHandler<AlcoholsWithPaginationCriteria, PaginatedList<AlcoholBriefDto>>
    {
        private readonly IAlcoholRepository _AlcoholRepository;
        private readonly IMapper _mapper;

        public GetAlcoholsWithPaginationQueryHandler(IAlcoholRepository AlcoholRepository, IMapper mapper)
        {
            _AlcoholRepository = AlcoholRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AlcoholBriefDto>> Handle(AlcoholsWithPaginationCriteria criteria, CancellationToken cancellationToken)
        {
            PaginatedList<Alcohol> Lista = await _AlcoholRepository.GetAlcoholsByCriteria(criteria);
            return new PaginatedList<AlcoholBriefDto>(_mapper.Map<List<Alcohol>, List<AlcoholBriefDto>>(Lista.Items), Lista.TotalCount, criteria.PageNumber, criteria.PageSize);

        }

    }
}
