using Application.Common.Utils;
using FluentValidation;

namespace Application.Entities.Commons
{
    public record PaginationQuery
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? SortBy { get; set; }
        public bool? SortAsc { get; set; }
    }
    public abstract class PaginationQueryValidator<T> : AbstractValidator<T> where T : PaginationQuery
    {
        public PaginationQueryValidator()
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
