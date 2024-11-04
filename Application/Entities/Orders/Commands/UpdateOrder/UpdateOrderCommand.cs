using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTO;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public int Id { get; }
        public OrdersUpdateDto orderToUpdate { get; }
        public UpdateOrderCommand(int id, OrdersUpdateDto request)
        {
            Id = id;
            orderToUpdate = request;
        }
    }
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IMediator mediator, ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _orderRepository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                _logger.LogInformation("[UpdateOrderCommandHandler] Order with Id={@Id} not found!", request.Id);
                throw new NotFoundException(nameof(Order), request.Id);
            }

            entity.Address = request.orderToUpdate.Address;
            entity.Status = request.orderToUpdate.Status;
            entity.Amount = request.orderToUpdate.Amount;
            entity.ModifiedBy = 1;
            entity.ModifiedDate = DateTime.Now;

            if (entity.ModifiedDate.HasValue)
            {
                DateTime dateTimeValue = DateTime.SpecifyKind(entity.ModifiedDate.Value, DateTimeKind.Utc);

                if (dateTimeValue.Kind == DateTimeKind.Local)
                    dateTimeValue = dateTimeValue.ToUniversalTime();

                entity.ModifiedDate = dateTimeValue;
            }

            await _orderRepository.UpdateAsync(entity);

            if (await _orderRepository.SaveChangesAsync(cancellationToken))
            {
                //await _mediator.Publish(new OrderUpdatedEvent(entity));
                _logger.LogInformation("[UpdateOrderCommandHandler] Updated successfully. returned {@toReturn} and sent notifications", entity);
            }
            else
            {
                _logger.LogInformation("[UpdateOrderCommandHandler] Update was not successful. returned {@toReturn}", entity);
            }


            return entity;
        }
    }
}
