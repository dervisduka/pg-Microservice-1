using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Orders.Queries
{
    public record OrdersWithPaginationCriteria : IRequest<PaginatedList<OrdersBriefDto>>
    {
        public int? Id { get; set; }
        public string? Address { get; set; }
        public bool UseAddressWithLike { get; set; } = false;
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public bool IsCreatedDateBetweenUsed { get; set; } = false;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

    public class GetOrdersWithPaginationQueryHandler : IRequestHandler<OrdersWithPaginationCriteria, PaginatedList<OrdersBriefDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersWithPaginationQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<OrdersBriefDto>> Handle(OrdersWithPaginationCriteria criteria, CancellationToken cancellationToken)
        {
            PaginatedList<Order> Lista = await _orderRepository.GetOrdersByCriteria(criteria);
            return new PaginatedList<OrdersBriefDto>(_mapper.Map<List<Order>, List<OrdersBriefDto>>(Lista.Items), Lista.TotalCount, criteria.PageNumber, criteria.PageSize);

        }

    }
}
