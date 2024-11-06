using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Bills.Queries
{
    public record BillsWithPaginationCriteria : IRequest<PaginatedList<BillsBriefDto>>
    {
        public int? Id { get; set; }
        public Decimal? Amount { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCreatedDateBetweenUsed { get; set; } = false;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

    public class GetBillsWithPaginationQueryHandler : IRequestHandler<BillsWithPaginationCriteria, PaginatedList<BillsBriefDto>>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public GetBillsWithPaginationQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BillsBriefDto>> Handle(BillsWithPaginationCriteria criteria, CancellationToken cancellationToken)
        {

            //ketu kthehet PaginatedList<Bill>
            PaginatedList<Bill> Lista = await _unit.Bill.GetBillsByCriteria(criteria);

            //bejme return versionin e mapuar
            return new PaginatedList<BillsBriefDto>(_mapper.Map<List<Bill>, List<BillsBriefDto>>(Lista.Items), Lista.TotalCount, criteria.PageNumber, criteria.PageSize);
        }
    }
}
