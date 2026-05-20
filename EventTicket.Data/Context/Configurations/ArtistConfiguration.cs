using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Bio)
            .HasMaxLength(2000);

        builder.Property(a => a.Genre)
            .HasMaxLength(100);

        builder.Property(a => a.ImageUrl)
            .HasMaxLength(500);

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
