using System;

namespace Domain
{
    public class Bid : AuditableEntity
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid? ItemId { get; set; }
        public Item Item { get; set; }
    }
}