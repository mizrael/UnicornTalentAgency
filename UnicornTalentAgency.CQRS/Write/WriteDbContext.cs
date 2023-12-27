using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Write.Entities;

namespace UnicornTalentAgency.CQRS.Write;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audition>()
            .HasKey(a => new { a.RoleId, a.UnicornId });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Audition> Auditions => Set<Audition>();
    public DbSet<Unicorn> Unicorns => Set<Unicorn>();
    public DbSet<CastingRole> Roles => Set<CastingRole>();
}
