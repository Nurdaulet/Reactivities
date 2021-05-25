
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder
                .ToTable("Pictures");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Url)
                .IsRequired();

            builder
                .Property(p => p.ItemId)
                .IsRequired();
        }
    }
}
