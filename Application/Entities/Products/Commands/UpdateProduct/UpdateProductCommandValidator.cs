using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using FluentValidation;

namespace Application.Entities.Products.Commands
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IUnitOfWork _unit;
        public UpdateProductCommandValidator(IUnitOfWork unit)
        {
            _unit = unit;

            RuleFor(v => v.product.Name)
                .MaximumLength(200)
                .WithErrorMaximumLength(200)
                .NotEmpty()
                .WithCustomErrorCode();

            RuleFor(r => r)
                 .MustAsync(BeUniqueName)
                 .WithMessage("The specified name already exists.")
                 .WithErrorCodeAlreadyExists("Name")
                 .WithCustomErrorCode();

        }

        public async Task<bool> BeUniqueName(UpdateProductCommand oBjekti, CancellationToken e)
        {
            //kuptohet qe kjo duhet permiresuar  qe mos behet get all por te behet get me criteria
            IReadOnlyList<Product> lista = await _unit.Products.GetAllAsync();

            return lista.Count(l => l.Name == oBjekti.product.Name && l.Id != oBjekti.product.Id) < 1;
        }
    }
}
