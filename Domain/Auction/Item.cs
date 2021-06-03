using System;
using System.Collections.Generic;

namespace Domain
{
    public class Item : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal MinIncrease { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsEmailSent { get; set; } = false;

        public string Username { get; set; }
        public AppUser User { get; set; }

        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
        public ICollection<Picture> Pictures { get; set; } = new HashSet<Picture>();
    }
}