

using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Items
{
    public class Details
    {
        public class Query : IRequest<Result<ItemDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ItemDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<ItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await _context
                    .Items
                    .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

                return Result<ItemDto>.Success(item);
            }
        }
    }
}
