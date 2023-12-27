using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence;

namespace UnicornTalentAgency.CRUD.Routes;

public static class UnicornRoutes
{
    public static WebApplication MapUnicornRoutes(this WebApplication app)
    {
        var api = app.MapGroup("/api/unicorns");

        api.MapGet("/", GetUnicorns);

        api.MapGet("/{id}", GetUnicorn);

        return app;
    }

    private static Task<UnicornArchiveDto[]> GetUnicorns(
        [FromServices] UTADbContext dbContext, CancellationToken cancellationToken = default)
        => dbContext.Unicorns
            .AsNoTracking()
            .Include(u => u.Auditions)
            .ThenInclude(a => a.Role)
            .Select(r => new UnicornArchiveDto(
                r.Id,
                r.Name,
                r.Auditions.Where(a => a.IsSuccessful)
                                .Sum(a => a.Role.Pay)
            )).ToArrayAsync(cancellationToken);

    private static async Task<UnicornDetailsDto?> GetUnicorn(
        int id,
        [FromServices] UTADbContext dbContext, 
        CancellationToken cancellationToken = default)
       {
            var entity = await dbContext.Unicorns
                .AsNoTracking()
                .Include(u => u.Auditions)
                .ThenInclude(a => a.Role)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            var successfulRoles = entity?.Auditions.Where(a => a.IsSuccessful)
                                                   .ToArray();

            return (entity is null) ? 
                null : 
                new UnicornDetailsDto(
                    entity.Id,
                    entity.Name,
                    entity.MagicalAbilities,
                    entity.Auditions.Count(),
                    successfulRoles?.Sum(a => a.Role.Pay) ?? 0,
                    successfulRoles?.Select(a => a.Role.Name).ToArray() ?? Array.Empty<string>()
                );
        }
}