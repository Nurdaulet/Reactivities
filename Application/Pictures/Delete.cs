

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Notifications.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Pictures
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public DeletePictureModel Picture { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Picture.ItemId).NotEmpty();
                RuleFor(x => x.Picture.PictureId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>,
        INotificationHandler<ItemDeletedNotification>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMediator _mediator;
            private readonly IPhotoAccessor _photoAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor, IMediator mediator)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mediator = mediator;
                _photoAccessor = photoAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var pictureToRemove = await _context
                    .Pictures
                    .Include(p => p.Item)
                    .Where(p => p.Id == request.Picture.PictureId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (
                    pictureToRemove == null
                    || pictureToRemove.Item.Username != _userAccessor.GetUsername()
                    || pictureToRemove.ItemId != request.Picture.ItemId)
                {
                    return null;
                }

                await _photoAccessor.DeleteResourcesByPrefixAsync($"{request.Picture.ItemId}/{request.Picture.PictureId}");
                _context.Pictures.Remove(pictureToRemove);
                if (await _context.SaveChangesAsync(cancellationToken) <= 0)
                {
                    return Result<Unit>.Failure("Failed to delete item");
                }

                var pictures = await _context
                    .Pictures
                    .Where(p => p.ItemId == request.Picture.ItemId)
                    .AnyAsync(cancellationToken);

                if (!pictures)
                {
                    await AddDefaultPicture(request.Picture.ItemId);
                }

                return Result<Unit>.Success(Unit.Value);
            }

            public async Task Handle(ItemDeletedNotification notification, CancellationToken cancellationToken)
            {
                await _photoAccessor.DeleteResourcesByPrefixAsync($"{notification.ItemId}/");
                await _photoAccessor.DeleteFolderAsync($"{notification.ItemId}");
            }

            private async Task AddDefaultPicture(Guid itemId)
                => await _mediator.Send(new CreatePictureModel { ItemId = itemId });
        }
    }
}
