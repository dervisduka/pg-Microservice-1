using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Bills.Commands
{
    public record CreateBillCommand : IRequest<Bill>
    {
        public Decimal Amount { get; set; }
        public int Status { get; set; }
    }

    public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, Bill>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICurrentActorService _currentUserService;

        public CreateBillCommandHandler(IUnitOfWork unit, IMediator mediator, ILogger<CreateBillCommandHandler> logger, ICurrentActorService currentUserService)
        {
            _unit = unit;
            _mediator = mediator;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<Bill> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {

            var entity = new Bill
            {
                Amount = request.Amount,
                Status = request.Status,

                CreatedDate = DateTime.Now
            };

            //int i = 0, j = 0;
            //int k = i / j;

            Bill toReturn = await _unit.Bill.AddAsyncTransactional(entity);
            if (await _unit.CommitThings())
            {
                //bejme publish domain events per cdo objekt qe mori pjese ne kete komande
                //ne menyre qe te kapet nga notification handles te interesuar
                await _mediator.Publish(new BillCreatedEvent(entity));
                _logger.LogInformation("[CreateBillCommandHandler] Commited successfully and sent notifications!");
            }

            return toReturn;

        }
    }
}
