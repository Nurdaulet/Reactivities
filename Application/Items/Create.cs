

using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Pictures;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Items
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public CreateItemModel Item { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IDateTime dateTime)
            {
                RuleFor(x => x.Item).SetValidator(new ItemValidator(dateTime));
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper, IMediator mediator)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mediator = mediator;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_userAccessor.GetUsername() == null
                    || !await _context.SubCategories.AnyAsync(c => c.Id == request.Item.SubCategoryId, cancellationToken))
                {
                    return Result<Unit>.Failure(ExceptionMessages.Item.CreateItemErrorMessage);
                }

                var item = _mapper.Map<Item>(request.Item);
                item.Username = _userAccessor.GetUsername();
                item.StartTime = item.StartTime.ToUniversalTime();
                item.EndTime = item.EndTime.ToUniversalTime();

                await _context.Items.AddAsync(item, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Send(new PictureDto { ItemId = item.Id, Pictures = request.Item.Pictures },
                    cancellationToken);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create item");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
