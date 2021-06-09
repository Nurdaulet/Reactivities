

using System;

namespace Application.Bid
{
    public class BidModel
    {
        public decimal Amount { get; set; }

        public Guid ItemId { get; set; }
        public string Username { get; set; }
    }
}
