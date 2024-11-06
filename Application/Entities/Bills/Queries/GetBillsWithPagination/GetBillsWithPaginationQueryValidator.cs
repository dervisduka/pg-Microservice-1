using Application.Common.Utils;
using FluentValidation;

namespace Application.Entities.Bills.Queries
{
    public class GetBillsWithPaginationQueryValidator : AbstractValidator<BillsWithPaginationCriteria>
    {
        public GetBillsWithPaginationQueryValidator()
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

        }
    }
}
