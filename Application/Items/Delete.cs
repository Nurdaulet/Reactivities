

using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Notifications.Models;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Items
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty(); 
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMediator _mediator;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMediator mediator, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mediator = mediator;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var itemToDelete = await _context
                    .Items
                    .FindAsync(request.Id);

                if (itemToDelete == null
                    || itemToDelete.Username != _userAccessor.GetUsername()) //&& !this.currentUserService.IsAdmin
                {
                    return null;
                }

                _context.Items.Remove(itemToDelete);
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the activity");
                await _mediator.Publish(new ItemDeletedNotification(itemToDelete.Id), cancellationToken);
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
