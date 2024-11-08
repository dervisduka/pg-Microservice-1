using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Alcohols.Commands
{
    public record DeleteAlcoholCommand(int Id) : IRequest<int>;

    public class DeleteAlcoholCommandHandler : IRequestHandler<DeleteAlcoholCommand, int>
    {
        private readonly IAlcoholRepository _AlcoholRepository;

        public DeleteAlcoholCommandHandler(IAlcoholRepository AlcoholRepository)
        {
            _AlcoholRepository = AlcoholRepository;
        }

        public async Task<int> Handle(DeleteAlcoholCommand request, CancellationToken cancellationToken)
        {
            var entity = await _AlcoholRepository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Alcohol), request.Id);
            }

            await _AlcoholRepository.DeleteAsync(request.Id);

            await _AlcoholRepository.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}
