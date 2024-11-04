using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Orders.Commands
{
    public record DeleteOrderCommand(int Id) : IRequest<int>;

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _orderRepository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await _orderRepository.DeleteAsync(request.Id);

            await _orderRepository.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}
