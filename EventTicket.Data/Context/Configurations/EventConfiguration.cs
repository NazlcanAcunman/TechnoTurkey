using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500);

        builder.Property(e => e.TicketPrice)
            .HasPrecision(18, 2);

        builder.Property(e => e.SoldCount)
            .HasDefaultValue(0);

        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Artist)
            .WithMany(a => a.Events)
            .HasForeignKey(e => e.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.IsApproved, e.Date, e.IsDeleted });
        builder.HasIndex(e => e.VenueId);
        builder.HasIndex(e => e.ArtistId);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
