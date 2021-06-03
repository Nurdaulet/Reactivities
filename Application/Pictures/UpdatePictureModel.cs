

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Application.Pictures
{
    public class UpdatePictureModel
    {
        public Guid ItemId { get; set; }

        public ICollection<IFormFile> PicturesToAdd { get; set; } = new HashSet<IFormFile>();

        public ICollection<Guid> PicturesToRemove { get; set; } = new HashSet<Guid>();
    }
}
