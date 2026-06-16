using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class GuestlistConfiguration : IEntityTypeConfiguration<GuestlistRequest>
{
    public void Configure(EntityTypeBuilder<GuestlistRequest> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.AddedByUserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(g => g.GuestName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.GuestPhone)
            .HasMaxLength(20);

        builder.Property(g => g.Note)
            .HasMaxLength(500);

        builder.Property(g => g.AdminNote)
            .HasMaxLength(1000);

        builder.Property(g => g.Status)
            .HasConversion<int>();

        builder.HasOne(g => g.Event)
            .WithMany()
            .HasForeignKey(g => g.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.AddedByUser)
            .WithMany()
            .HasForeignKey(g => g.AddedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => new { g.EventId, g.AddedByUserId, g.GuestName })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(g => new { g.EventId, g.Status, g.IsDeleted });

        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}
