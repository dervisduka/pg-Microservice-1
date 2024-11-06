using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Bills.Commands
{
    public record DeleteBillCommand(int Id) : IRequest<int>;

    public class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand, int>
    {
        private readonly IUnitOfWork _unit;

        public DeleteBillCommandHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<int> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unit.Bill
                .GetByIdAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Bill), request.Id);
            }


            //entity.AddDomainEvent(new BillDeletedEvent(entity));

            return await _unit.Bill.DeleteAsync(entity.Id);
        }
    }
}
