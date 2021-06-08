
using Application.Core;
using FluentValidation;

namespace Application.Bid
{
    public class CreateBidValidator : AbstractValidator<CreateBidModel>
    {
        public CreateBidValidator()
        {
            RuleFor(p => p.Amount).NotEmpty()
                .InclusiveBetween(ModelConstants.Bid.MinAmount, ModelConstants.Bid.MaxAmount);
            RuleFor(p => p.ItemId).NotEmpty();
            RuleFor(p => p.Username).NotEmpty();
        }
    }
}
