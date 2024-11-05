using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Bills.Queries
{
    public record GetAllBillsEmptyCriteria() : IRequest<IReadOnlyList<Bill>>;


    public class GetAllBillsQueryHandler : IRequestHandler<GetAllBillsEmptyCriteria, IReadOnlyList<Bill>>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public GetAllBillsQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Bill>> Handle(GetAllBillsEmptyCriteria criteria, CancellationToken cancellationToken)
        {

            var data = await _unit.Bill.GetAllAsync();

            //bejme return versionin e mapuar
            return data;
        }
    }
}
