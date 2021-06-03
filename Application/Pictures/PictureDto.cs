

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Application.Pictures
{
    public class PictureDto
    {
        public Guid ItemId { get; set; }

        public ICollection<IFormFile> Pictures { get; set; } = new HashSet<IFormFile>();
    }
}
