
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Pictures;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Items
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UpdateItemModel UpdateItem { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IDateTime dateTime)
            {
                RuleFor(x => x.UpdateItem).SetValidator(new UpdateItemValidator(dateTime));
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMediator _mediator;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IMediator mediator)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mediator = mediator;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = await _context
                    .Items
                    .SingleOrDefaultAsync(i => i.Id == request.UpdateItem.Id, cancellationToken);

                if (item == null
                    || item.Username != _userAccessor.GetUsername())
                {
                    return null;
                }

                if (!await _context
                    .SubCategories
                    .AnyAsync(c => c.Id == request.UpdateItem.SubCategoryId, cancellationToken))
                {
                    return Result<Unit>.Failure(ExceptionMessages.Item.SubCategoryDoesNotExist);
                }

                item.Title = request.UpdateItem.Title;
                item.Description = request.UpdateItem.Description;
                item.StartingPrice = request.UpdateItem.StartingPrice;
                item.MinIncrease = request.UpdateItem.MinIncrease;
                item.StartTime = request.UpdateItem.StartTime.ToUniversalTime();
                item.EndTime = request.UpdateItem.EndTime.ToUniversalTime();
                item.SubCategoryId = request.UpdateItem.SubCategoryId;

                _context.Items.Update(item);
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Send(new UpdatePictureModel
                    {
                        ItemId = item.Id,
                        PicturesToAdd = request.UpdateItem.PicturesToAdd,
                        PicturesToRemove = request.UpdateItem.PicturesToRemove
                    },
                    cancellationToken);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
