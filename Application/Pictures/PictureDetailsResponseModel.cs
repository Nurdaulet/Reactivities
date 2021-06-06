

using System;

namespace Application.Pictures
{
    public class PictureDetailsResponseModel
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public Guid ItemId { get; set; }

        public string ItemUserId { get; set; }
    }
}
