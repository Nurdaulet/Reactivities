

using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
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
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);
                if (activity == null) return null;

                _mapper.Map(request.Activity, activity);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to edit activity");
                return Result<Unit>.Success(Unit.Value);


                var itemToDelete = await _context
                    .Items
                    .FindAsync(request.Id);

                if (itemToDelete == null
                    || itemToDelete.UserId != this.currentUserService.UserId && !this.currentUserService.IsAdmin)
                {
                    throw new NotFoundException(nameof(Item));
                }

                this.context.Items.Remove(itemToDelete);
                await this.context.SaveChangesAsync(cancellationToken);
                await this.mediator.Publish(new ItemDeletedNotification(itemToDelete.Id), cancellationToken);
            }
        }
    }
}
