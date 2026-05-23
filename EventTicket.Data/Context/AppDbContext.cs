using EventTicket.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventTicket.Data.Context;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ContactForm> ContactForms { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<AcademyContent> AcademyContents { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<PageView> PageViews { get; set; }
    public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    
                    if (entry.Entity.IsDeleted && entry.Entity.DeletedAt == null)
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}