
using System;

namespace Application.Bid
{
    public class GetHighestBidDetailsResponseModel
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string UserId { get; set; }

        public Guid ItemId { get; set; }
    }
}
