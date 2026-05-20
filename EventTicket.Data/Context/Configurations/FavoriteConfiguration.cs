using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(f => f.Venue)
            .WithMany()
            .HasForeignKey(f => f.VenueId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(f => f.Artist)
            .WithMany()
            .HasForeignKey(f => f.ArtistId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(f => new { f.UserId, f.VenueId })
            .IsUnique()
            .HasFilter("\"VenueId\" IS NOT NULL");

        builder.HasIndex(f => new { f.UserId, f.ArtistId })
            .IsUnique()
            .HasFilter("\"ArtistId\" IS NOT NULL");

        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}