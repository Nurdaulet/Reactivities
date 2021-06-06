
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Pictures
{
    public class Create
    {
        public class Command : IRequest<Result<List<PictureResponseModel>>>
        {
            public CreatePictureModel Picture { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Picture.ItemId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<List<PictureResponseModel>>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper, IPhotoAccessor photoAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                _photoAccessor = photoAccessor;
                _mapper = mapper;
            }

            public async Task<Result<List<PictureResponseModel>>> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = await _context
                    .Items
                    .Select(i => new
                    {
                        i.Id,
                        i.Username
                    })
                    .SingleOrDefaultAsync(i => i.Id == request.Picture.ItemId, cancellationToken);
                if (item.Username != _userAccessor.GetUsername())
                {
                    return null;
                }

                if (!request.Picture.Pictures.Any())
                {
                    return Result<List<PictureResponseModel>>.Success(new List<PictureResponseModel>());
                }

                var uploadResults = new ConcurrentBag<ImageUploadResult>();
                foreach (var picture in request.Picture.Pictures)
                {
                    var guid = Guid.NewGuid().ToString();
                    var uploadParams = new ImageUploadParams
                    {
                        PublicId = Guid.NewGuid().ToString(),
                        File = new FileDescription(guid, picture.OpenReadStream()),
                        Folder = $"{request.Picture.ItemId}"
                    };
                    var uploadResult = await _photoAccessor.UploadAsync(uploadParams);
                    uploadResults.Add(uploadResult);
                }

                var picturesToAdd = uploadResults.Select(picture => new Picture
                {
                    Id = Guid.Parse(picture.PublicId.Substring(picture.PublicId.LastIndexOf('/') + 1)),
                    ItemId = request.Picture.ItemId,
                    Url = picture.SecureUrl.AbsoluteUri
                }).ToList();

                await _context.Pictures.AddRangeAsync(picturesToAdd, cancellationToken);
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (result)
                {
                    return Result<List<PictureResponseModel>>.Success(picturesToAdd
                        .Select(p => _mapper.Map<PictureResponseModel>(p)).ToList());
                }
                return Result<List<PictureResponseModel>>.Failure("Problem adding pictures");
            }
        }
    }
}
