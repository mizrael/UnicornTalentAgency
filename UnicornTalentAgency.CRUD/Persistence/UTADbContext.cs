using System.Globalization;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence.Entities;

namespace UnicornTalentAgency.CRUD.Persistence;

public class UTADbContext : DbContext
{
    public UTADbContext(DbContextOptions<UTADbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audition>()
            .HasKey(a => new { a.CastingCallId, a.UnicornId });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Audition> Auditions => Set<Audition>();
    public DbSet<Unicorn> Unicorns => Set<Unicorn>();
    public DbSet<CastingRole> Roles => Set<CastingRole>();
}
