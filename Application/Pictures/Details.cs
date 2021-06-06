

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

namespace Application.Pictures
{
    public class Details
    {
        public class Query : IRequest<Result<PictureDetailsResponseModel>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PictureDetailsResponseModel>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<PictureDetailsResponseModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var picture = await _context
                    .Pictures
                    .Where(p => p.Id == request.Id)
                    .ProjectTo<PictureDetailsResponseModel>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

                if (picture == null)
                {
                    return null;
                }

                return Result<PictureDetailsResponseModel>.Success(picture);
            }
        }
    }
}
