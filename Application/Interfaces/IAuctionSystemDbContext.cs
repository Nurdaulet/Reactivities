using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}