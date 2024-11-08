using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Entities.Alcohols.Commands
{
    public record CreateAlcoholCommand : IRequest<Alcohol>
    {
        public Decimal Amount { get; set; }
        public int Status { get; set; }

        public Decimal AlcoholPercentage { get; set; }

        public Decimal PricePerBottle { get; set; }

    }

    public class CreateAlcoholCommandHandler : IRequestHandler<CreateAlcoholCommand, Alcohol>
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICurrentActorService _currentUserService;
        private readonly IAlcoholRepository _AlcoholRepository;

        public CreateAlcoholCommandHandler(IMediator mediator, ILogger<CreateAlcoholCommandHandler> logger, ICurrentActorService currentUserService, IAlcoholRepository AlcoholRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _currentUserService = currentUserService;
            _AlcoholRepository = AlcoholRepository;
        }

        public async Task<Alcohol> Handle(CreateAlcoholCommand request, CancellationToken cancellationToken)
        {

            var entity = new Alcohol
            {
                Amount = request.Amount,
                Status = request.Status,
                AlcoholPercentage = request.AlcoholPercentage,
                PricePerBottle = request.PricePerBottle,
            };


            await _AlcoholRepository.AddAsync(entity);

            if (await _AlcoholRepository.SaveChangesAsync(cancellationToken))
            {
                await _mediator.Publish(new AlcoholCreatedEvent(entity));
                _logger.LogInformation("[CreateAlcoholCommandHandler] Commited successfully and sent notifications!");
            }

            return entity;

        }
    }
}
