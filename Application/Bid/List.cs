
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Bid
{
    public class List
    {
        public class Query : IRequest<Result<List<BidModel>>>
        {
            public Guid ItemId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<BidModel>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<BidModel>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await _context.Bids
                    .Where(x => x.Item.Id == request.ItemId)
                    .OrderByDescending(x => x.Created)
                    .ProjectTo<BidModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<BidModel>>.Success(comments);
            }
        }
    }
}
