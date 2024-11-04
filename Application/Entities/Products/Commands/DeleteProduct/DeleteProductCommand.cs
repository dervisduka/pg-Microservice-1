using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<int>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IUnitOfWork _unit;

        public DeleteProductCommandHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unit.Products
                .GetByIdAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }


            //entity.AddDomainEvent(new ProductDeletedEvent(entity));

            return await _unit.Products.DeleteAsync(entity.Id);
        }
    }
}
