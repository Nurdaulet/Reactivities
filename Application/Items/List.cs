using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Items
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ListItems>>>
        {
            public ItemParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ListItems>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<PagedList<ListItems>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var query = _context.Activities
                //.Where(d => d.Date >= request.Params.StartDate)
                //.OrderBy(d => d.Date)
                //.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                //    new { currentUsername = _userAccessor.GetUsername() })
                //.AsQueryable();

                var query = _context
                    .Items
                    .Include(i => i.Pictures)
                    .Include(u => u.User)
                    .AsQueryable();

                query = this.AddFiltersOnQuery(request.Params, query);

                return Result<PagedList<ListItems>>.Success(await PagedList<ListItems>.CreateAsync(query,
                     request.Params.PageNumber, request.Params.PageSize));
            }

            private IQueryable<Item> AddFiltersOnQuery(ItemParams filters, IQueryable<Item> queryable)
            {
                if (!string.IsNullOrEmpty(filters?.Title))
                {
                    queryable = queryable.Where(i => i.Title.ToLower().Contains(filters.Title.ToLower()));
                }

                if (!string.IsNullOrEmpty(filters?.UserId))
                {
                    queryable = queryable.Where(i => i.UserId == filters.UserId);
                }

                if (filters?.GetLiveItems == true)
                {
                    queryable = queryable.Where(i =>
                        i.StartTime < this.dateTime.UtcNow && i.EndTime > this.dateTime.UtcNow);
                }

                if (filters?.MinPrice != null)
                {
                    queryable = queryable.Where(i => i.StartingPrice >= filters.MinPrice);
                }

                if (filters?.MaxPrice != null)
                {
                    queryable = queryable.Where(i => i.StartingPrice <= filters.MaxPrice);
                }

                if (filters?.StartTime != null)
                {
                    queryable = queryable.Where(i => i.StartTime >= filters.StartTime.Value.ToUniversalTime());
                }

                if (filters?.EndTime != null)
                {
                    queryable = queryable.Where(i => i.EndTime <= filters.EndTime.Value.ToUniversalTime());
                }

                if (filters?.MinimumPicturesCount != null)
                {
                    queryable = queryable.Where(i => i.Pictures.Count >= filters.MinimumPicturesCount);
                }

                if (filters?.SubCategoryId != Guid.Empty)
                {
                    queryable = queryable.Where(i => i.SubCategoryId == filters.SubCategoryId);
                }

                return queryable;
            }
        }
    }
}