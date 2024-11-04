using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Products.Commands
{
    public record UpdateProductCommand(Product product) : IRequest<Product>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public UpdateProductCommandHandler(IUnitOfWork unit, IMediator mediator, ILogger<UpdateProductCommandHandler> logger)
        {
            _unit = unit;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unit.Products
                .GetByIdAsync(request.product.Id);

            if (entity == null)
            {
                _logger.LogInformation("[UpdateProductCommandHandler] Product with Id={@Id} not found!", request.product.Id);
                throw new NotFoundException(nameof(Product), request.product.Id);
            }

            entity.Name = request.product.Name;
            entity.Description = request.product.Description;
            entity.Barcode = request.product.Barcode;
            entity.Rate = request.product.Rate;



            Product toReturn = await _unit.Products.UpdateAsync(entity);
            if (toReturn != null)
            {
                await _mediator.Publish(new ProductUpdatedEvent(entity));
                _logger.LogInformation("[UpdateProductCommandHandler] Updated successfully. returned {@toReturn} and sent notifications", toReturn);
            }
            else
            {
                _logger.LogInformation("[UpdateProductCommandHandler] Update was not successful. returned {@toReturn}", toReturn);
            }


            return toReturn;
        }
    }
}
