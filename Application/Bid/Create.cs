

using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Bid
{
    public class Create
    {

        public class Command : IRequest<Result<Unit>>
        {
            public CreateBidModel CreateBid { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new CreateBidValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                await CheckWhetherItemIsEligibleForBidding(request.CreateBid, cancellationToken);

                var bid = _mapper.Map<Domain.Bid>(request);
                await _context.Bids.AddAsync(bid, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Unit>.Success(Unit.Value);
            }

            private async Task<Result<Unit>> CheckWhetherItemIsEligibleForBidding(CreateBidModel request,
                CancellationToken cancellationToken)
            {
                var item = await _context
                    .Items
                    .Select(i => new
                    {
                        i.Id,
                        i.StartingPrice,
                        i.StartTime,
                        i.EndTime,
                        HighestBidAmount = i.Bids
                            .Select(b => b.Amount)
                            .OrderByDescending(amount => amount)
                            .FirstOrDefault()
                    })
                    .Where(i => i.Id == request.ItemId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (item == null)
                {
                    return null;
                }

                if (request.Username != _userAccessor.GetUsername())
                {
                    return null;
                }

                if (item.StartTime >= this.dateTime.UtcNow)
                {
                    //Bid hasn't started yet.
                    throw new BadRequestException(
                        string.Format(ExceptionMessages.Bid.BiddingNotStartedYet, request.ItemId));
                }

                if (item.EndTime <= this.dateTime.UtcNow)
                {
                    // Bidding has ended
                    throw new BadRequestException(
                        string.Format(ExceptionMessages.Bid.BiddingHasEnded, request.ItemId));
                }

                if (request.Amount <= item.HighestBidAmount
                    || request.Amount <= item.StartingPrice)
                {
                    throw new BadRequestException(ExceptionMessages.Bid.InvalidBidAmount);
                }
            }
        }
    }
}
