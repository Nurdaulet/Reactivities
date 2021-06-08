

using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
namespace Application.Bid
{
    public class GetHighestBidDetails
    {
        public class Query : IRequest<Result<GetHighestBidDetailsResponseModel>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<GetHighestBidDetailsResponseModel>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<GetHighestBidDetailsResponseModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var bid = await _context
                    .Bids
                    .Where(b => b.ItemId == request.Id)
                    .OrderByDescending(b => b.Amount)
                    .ProjectTo<GetHighestBidDetailsResponseModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return Result<GetHighestBidDetailsResponseModel>.Success(bid);
            }
        }
    }
}
