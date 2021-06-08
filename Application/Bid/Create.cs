

using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
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
                RuleFor(x => x.CreateBid).SetValidator(new CreateBidValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            private readonly IDateTime _dateTime;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper, IDateTime dateTime)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
                _dateTime = dateTime;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await CheckWhetherItemIsEligibleForBidding(request.CreateBid, cancellationToken);
                if (!result.IsSuccess)
                {
                    return result;
                }
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

                if (item.StartTime >= _dateTime.UtcNow)
                {
                    //Bid hasn't started yet.
                    return Result<Unit>.Failure(ExceptionMessages.Bid.BiddingNotStartedYet);
                }

                if (item.EndTime <= _dateTime.UtcNow)
                {
                    // Bidding has ended
                    return Result<Unit>.Failure(ExceptionMessages.Bid.BiddingHasEnded);
                }

                if (request.Amount <= item.HighestBidAmount
                    || request.Amount <= item.StartingPrice)
                {
                    return Result<Unit>.Failure(ExceptionMessages.Bid.InvalidBidAmount);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
