using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public ICollection<ActivityAttendee> Activities { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserFollowing> Followings { get; set; }
        public ICollection<UserFollowing> Followers { get; set; }
        public string FullName { get; set; }
        public ICollection<Item> ItemsSold { get; set; } = new HashSet<Item>();
        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
    }
}