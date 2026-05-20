using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.Description)
            .HasMaxLength(2000);

        builder.Property(v => v.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Address)
            .HasMaxLength(500);

        builder.Property(v => v.AdminUserId)
            .HasMaxLength(450);

        builder.HasIndex(v => v.AdminUserId);

        builder.HasIndex(v => new { v.City, v.IsApproved, v.IsDeleted });

        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
