using Application.Common.Utils;
using FluentValidation;

namespace Application.Entities.Orders.Queries
{
    public class GetOrdersWithPaginationQueryValidator : AbstractValidator<OrdersWithPaginationCriteria>
    {
        public GetOrdersWithPaginationQueryValidator()
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

            RuleFor(x => new { x.Address, x.UseAddressWithLike })
                .Must(x => AddressIfLike(x.Address, x.UseAddressWithLike))
                .WithMessage("Address must have a value if UseAddressWithLike is true!")
                .WithCustomErrorCode(null, "Address_Must_Have_Value");
        }


        public bool AddressIfLike(string address, bool UseAddressWithLike)
        {
            if (UseAddressWithLike)
                if (address == null)
                    return false;

            return true;
        }
    }
}
