using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Application.Items
{
    public class CreateItemModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid SubCategoryId { get; set; }

        public ICollection<IFormFile> Pictures { get; set; } = new HashSet<IFormFile>();
    }
}
