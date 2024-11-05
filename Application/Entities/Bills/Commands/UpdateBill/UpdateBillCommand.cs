using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Bills.Commands
{
    public record UpdateBillCommand(Bill Bill) : IRequest<Bill>;

    public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand, Bill>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICurrentActorService _currentUserService;


        public UpdateBillCommandHandler(IUnitOfWork unit, IMediator mediator, ILogger<UpdateBillCommandHandler> logger, ICurrentActorService currentUserService)
        {
            _unit = unit;
            _mediator = mediator;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<Bill> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unit.Bill
                .GetByIdAsync(request.Bill.Id);

            if (entity == null)
            {
                _logger.LogInformation("[UpdateBillCommandHandler] Bill with Id={@Id} not found!", request.Bill.Id);
                throw new NotFoundException(nameof(Bill), request.Bill.Id);
            }

            entity.Amount = request.Bill.Amount;
            entity.Status = request.Bill.Status;
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedBy = Int32.Parse(_currentUserService.UserId);




            Bill toReturn = await _unit.Bill.UpdateAsync(entity);
            if (toReturn != null)
            {
                await _mediator.Publish(new BillUpdatedEvent(entity));
                _logger.LogInformation("[UpdateBillCommandHandler] Updated successfully. returned {@toReturn} and sent notifications", toReturn);
            }
            else
            {
                _logger.LogInformation("[UpdateBillCommandHandler] Update was not successful. returned {@toReturn}", toReturn);
            }


            return toReturn;
        }
    }
}
