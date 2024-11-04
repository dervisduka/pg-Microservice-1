using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Orders.Queries
{
    public record GetAllOrdersEmptyCriteria() : IRequest<IReadOnlyList<Order>>;


    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersEmptyCriteria, IReadOnlyList<Order>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Order>> Handle(GetAllOrdersEmptyCriteria criteria, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetAllAsync();
        }

    }
}
