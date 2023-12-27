using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Read.Queries;

namespace UnicornTalentAgency.CQRS.Read;

public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<UnicornArchiveItem> UnicornArchive => Set<UnicornArchiveItem>();
    public DbSet<UnicornDetails> UnicornDetails => Set<UnicornDetails>();
    public DbSet<CastingRoleArchive> CastingRoleArchive => Set<CastingRoleArchive>();
    public DbSet<CastingRoleDetails> CastingRoleDetails => Set<CastingRoleDetails>();
}
