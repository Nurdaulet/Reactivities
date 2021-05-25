
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Amount)
                .IsRequired();

            builder
                .Property(p => p.UserId)
                .IsRequired();

            builder
                .Property(p => p.ItemId);
        }
    }
}
