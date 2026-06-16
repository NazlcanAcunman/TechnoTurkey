using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class GuestlistConfiguration : IEntityTypeConfiguration<GuestlistRequest>
{
    public void Configure(EntityTypeBuilder<GuestlistRequest> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(g => g.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(g => g.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(g => g.Note)
            .HasMaxLength(500);

        builder.Property(g => g.AdminNote)
            .HasMaxLength(1000);

        builder.Property(g => g.Status)
            .HasConversion<int>();

        builder.Property(g => g.GuestlistType)
            .HasConversion<int>();

        builder.HasOne(g => g.Event)
            .WithMany()
            .HasForeignKey(g => g.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.User)
            .WithMany()
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Bir kullanıcı aynı etkinliğe yalnızca bir kez başvurabilir
        builder.HasIndex(g => new { g.EventId, g.UserId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(g => new { g.EventId, g.Status, g.IsDeleted });

        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}

