using EventTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTicket.Data.Context.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(c => c.UserFullName)
            .HasMaxLength(200);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.Rating)
            .HasDefaultValue(1);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Comment_Rating",
            "[Rating] >= 1 AND [Rating] <= 5"));

        builder.HasOne(c => c.Event)
            .WithMany()
            .HasForeignKey(c => c.EventId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Venue)
            .WithMany(v => v.Comments)
            .HasForeignKey(c => c.VenueId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => new { c.EventId, c.IsDeleted });
        builder.HasIndex(c => new { c.VenueId, c.IsDeleted });

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}