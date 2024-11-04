using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Orders.Commands
{
    public record CreateOrderCommand : IRequest<Order>
    {
        public Decimal Amount { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }

    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICurrentActorService _currentUserService;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IMediator mediator, ILogger<CreateOrderCommandHandler> logger, ICurrentActorService currentUserService, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _currentUserService = currentUserService;
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {

            var entity = new Order
            {
                Address = request.Address,
                Amount = request.Amount,
                Status = request.Status,
                // CreatedBy = Int32.Parse( _currentUserService.UserId),
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };


            await _orderRepository.AddAsync(entity);

            if (await _orderRepository.SaveChangesAsync(cancellationToken))
            {
                await _mediator.Publish(new OrderCreatedEvent(entity));
                _logger.LogInformation("[CreateOrderCommandHandler] Commited successfully and sent notifications!");
            }

            return entity;

        }
    }
}
