using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Products.Commands
{
    public record CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Rate { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICurrentActorService _currentUserService;

        public CreateProductCommandHandler(IUnitOfWork unit, IMediator mediator, ILogger<CreateProductCommandHandler> logger, ICurrentActorService currentUserService)
        {
            _unit = unit;
            _mediator = mediator;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var entity = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Barcode = request.Barcode,
                Rate = request.Rate,
                CreatedBy = Int32.Parse(_currentUserService.UserId),
                CreatedDate = DateTime.Now
            };


            Product toReturn = await _unit.Products.AddAsyncTransactional(entity);

            //int i = 0, j = 0;
            //int k = i / j;

            Product toReturn2 = await _unit.Products2.AddAsyncTransactional(entity);
            if (await _unit.CommitThings())
            {
                //bejme publish domain events per cdo objekt qe mori pjese ne kete komande
                //ne menyre qe te kapet nga notification handles te interesuar
                await _mediator.Publish(new ProductCreatedEvent(entity));
                _logger.LogInformation("[CreateProductCommandHandler] Commited successfully and sent notifications!");
            }

            return toReturn;

        }
    }
}
