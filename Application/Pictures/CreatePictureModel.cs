

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Application.Pictures
{
    public class CreatePictureModel
    {
        public Guid ItemId { get; set; }

        public ICollection<IFormFile> Pictures { get; set; } = new HashSet<IFormFile>();
    }
}
