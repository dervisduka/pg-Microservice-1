using Application.Common.Utils;
using FluentValidation;

namespace Application.Entities.Products.Queries
{
    public class GetProductsWithPaginationQueryValidator : AbstractValidator<ProductsWithPaginationCriteria>
    {
        public GetProductsWithPaginationQueryValidator()
        {

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithErrorGreaterThanOrEqualsNumber(1)
                .WithMessage("PageNumber at least greater than or equal to 1.")
                .WithCustomErrorCode();

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithErrorGreaterThanOrEqualsNumber(1)
                .WithMessage("PageSize at least greater than or equal to 1.")
                .WithCustomErrorCode();

            RuleFor(x => new { x.Name, x.UseNameWithLike })
                .Must(x => NameIfLike(x.Name, x.UseNameWithLike))
                .WithMessage("Name must have a value if UseNameWithLike is true!")
                .WithCustomErrorCode(null, "Name_Must_Have_Value");
        }


        public bool NameIfLike(string name, bool UseNameWithLike)
        {
            if (UseNameWithLike)
                if (name == null)
                    return false;

            return true;
        }
    }
}
