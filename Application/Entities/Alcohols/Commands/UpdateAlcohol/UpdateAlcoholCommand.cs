using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTO;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Alcohols.Commands
{
    public class UpdateAlcoholCommand : IRequest<Alcohol>
    {
        public int Id { get; }
        public AlcoholUpdateDto AlcoholToUpdate { get; }
        public UpdateAlcoholCommand(int id, AlcoholUpdateDto request)
        {
            Id = id;
            AlcoholToUpdate = request;
        }
    }
    public class UpdateAlcoholCommandHandler : IRequestHandler<UpdateAlcoholCommand, Alcohol>
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IAlcoholRepository _AlcoholRepository;

        public UpdateAlcoholCommandHandler(IMediator mediator, ILogger<UpdateAlcoholCommandHandler> logger, IAlcoholRepository AlcoholRepository)
        {
            _AlcoholRepository = AlcoholRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Alcohol> Handle(UpdateAlcoholCommand request, CancellationToken cancellationToken)
        {
            var entity = await _AlcoholRepository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                _logger.LogInformation("[UpdateAlcoholCommandHandler] Alcohol with Id={@Id} not found!", request.Id);
                throw new NotFoundException(nameof(Alcohol), request.Id);
            }

            entity.Amount = request.AlcoholToUpdate.Amount;
            entity.Status = request.AlcoholToUpdate.Status;
            entity.AlcoholPercentage = request.AlcoholToUpdate.AlcoholPercentage;
            entity.PricePerBottle = request.AlcoholToUpdate.PricePerBottle;

            await _AlcoholRepository.UpdateAsync(entity);

            if (await _AlcoholRepository.SaveChangesAsync(cancellationToken))
            {
                //await _mediator.Publish(new AlcoholUpdatedEvent(entity));
                _logger.LogInformation("[UpdateAlcoholCommandHandler] Updated successfully. returned {@toReturn} and sent notifications", entity);
            }
            else
            {
                _logger.LogInformation("[UpdateAlcoholCommandHandler] Update was not successful. returned {@toReturn}", entity);
            }


            return entity;
        }
    }
}
