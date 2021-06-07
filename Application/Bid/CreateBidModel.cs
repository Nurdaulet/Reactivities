

using System;

namespace Application.Bid
{
    public class CreateBidModel
    {
        public decimal Amount { get; set; }

        public Guid ItemId { get; set; }
        public string Username { get; set; }
    }
}
