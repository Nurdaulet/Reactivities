
using System;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>, IAuctionSystemDbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(builder);
        }
    }

    public interface IAuctionSystemDbContext
    {
        DbSet<Category> Categories { get; set; }

        DbSet<SubCategory> SubCategories { get; set; }

        DbSet<Item> Items { get; set; }

        DbSet<Bid> Bids { get; set; }

        DbSet<Picture> Pictures { get; set; }

        DbSet<AppUser> Users { get; set; }
        //DbSet<SaldoUser> SaldoUsers { get; set; }

        // DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}