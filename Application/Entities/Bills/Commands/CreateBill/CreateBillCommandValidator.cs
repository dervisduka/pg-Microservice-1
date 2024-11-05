using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using FluentValidation;

namespace Application.Entities.Bills.Commands
{
    public class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
    {
        private readonly IUnitOfWork _unit;
        public CreateBillCommandValidator(IUnitOfWork unit)
        {
            _unit = unit;

            RuleFor(v => v.Amount)
                .Must(BillBiggerThan10)
                .WithMessage("The specified name already exists.")
                .WithErrorCodeAlreadyExists()
                .WithCustomErrorCode();

        }


        public bool BillBiggerThan10(Decimal amount)
        {
            return amount > 10;
        }
    }
}
