using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using FluentValidation;

namespace Application.Entities.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IUnitOfWork _unit;
        public CreateProductCommandValidator(IUnitOfWork unit)
        {
            _unit = unit;

            RuleFor(v => v.Name)
                .NotEmpty()
                .MaximumLength(200)
                .WithErrorMaximumLength(200)
                .MustAsync(BeUniqueName)
                .WithMessage("The specified name already exists.")
                .WithErrorCodeAlreadyExists()
                .WithCustomErrorCode();

        }


        public async Task<bool> BeUniqueName(string name, CancellationToken t)
        {
            IReadOnlyList<Product> lista = await _unit.Products.GetAllAsync();

            return lista.Count(l => l.Name == name) < 1;
        }
    }
}
