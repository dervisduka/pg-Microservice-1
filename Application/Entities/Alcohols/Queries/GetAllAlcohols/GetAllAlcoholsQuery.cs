using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Alcohols.Queries
{
    public record GetAllAlcoholsEmptyCriteria() : IRequest<IReadOnlyList<Alcohol>>;


    public class GetAllAlcoholsQueryHandler : IRequestHandler<GetAllAlcoholsEmptyCriteria, IReadOnlyList<Alcohol>>
    {
        private readonly IAlcoholRepository _AlcoholRepository;
        private readonly IMapper _mapper;

        public GetAllAlcoholsQueryHandler(IAlcoholRepository AlcoholRepository, IMapper mapper)
        {
            _AlcoholRepository = AlcoholRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Alcohol>> Handle(GetAllAlcoholsEmptyCriteria criteria, CancellationToken cancellationToken)
        {
            return await _AlcoholRepository.GetAllAsync();
        }

    }
}
