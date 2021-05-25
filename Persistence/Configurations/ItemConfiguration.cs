
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder
                .ToTable("Items");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(120);

            builder
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder
                .Property(p => p.StartingPrice)
                .IsRequired();

            builder
                .Property(p => p.MinIncrease)
                .IsRequired();

            builder
                .Property(p => p.StartTime)
                .IsRequired();

            builder
                .Property(p => p.EndTime)
                .IsRequired();

            builder
                .Property(p => p.IsEmailSent)
                .IsRequired();

            builder
                .Property(p => p.UserId)
                .IsRequired();

            builder
                .Property(p => p.SubCategoryId)
                .IsRequired();

            builder
                .HasMany(b => b.Bids)
                .WithOne(i => i.Item)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(b => b.Pictures)
                .WithOne(i => i.Item)
                .HasForeignKey(i => i.ItemId);
        }
    }
}
