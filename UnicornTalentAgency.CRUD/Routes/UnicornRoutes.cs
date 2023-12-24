using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence;

namespace UnicornTalentAgency.CRUD.Routes;

public static class UnicornRoutes
{
    public static WebApplication MapUnicornRoutes(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1/unicorns");

        api.MapGet("/", GetUnicorns)
            .WithName(nameof(GetUnicorns))
            .WithOpenApi();

        api.MapGet("/{id}", GetUnicorn)
            .WithName(nameof(GetUnicorn))
            .WithOpenApi();

        return app;
    }

    private static Task<UnicornArchiveDto[]> GetUnicorns(
        [FromServices] UTADbContext dbContext, CancellationToken cancellationToken = default)
        => dbContext.Unicorns
            .AsNoTracking()
            .Select(r => new UnicornArchiveDto(
                r.Id,
                r.Name
            )).ToArrayAsync(cancellationToken);

    private static async Task<UnicornDetailsDto?> GetUnicorn(
        int id,
        [FromServices] UTADbContext dbContext, 
        CancellationToken cancellationToken = default)
       {
            var entity = await dbContext.Unicorns
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            return (entity is null) ? 
                null : 
                new UnicornDetailsDto(
                    entity.Id,
                    entity.Name,
                    entity.MagicalAbilities
                );
        }
}