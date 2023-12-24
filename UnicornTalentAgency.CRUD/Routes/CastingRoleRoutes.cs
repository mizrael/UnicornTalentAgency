using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence;
using UnicornTalentAgency.CRUD.Persistence.Entities;

namespace UnicornTalentAgency.CRUD.Routes;

public static class CastingRoleRoutes
{
    public static WebApplication MapCastingRoleRoutes(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1/roles");

        api.MapGet("/", GetCastingRoles);

        api.MapGet("/{id:int}", GetCastingRole);

        api.MapPost("/{id:int}/audition/{unicornId:int}", Audition);

        api.MapPost("/{id:int}/audition/{unicornId:int}/select", SelectUnicorn);

        return app;
    }

    private static async Task<IActionResult> SelectUnicorn(
        int id,
        int unicornId,
        [FromServices] UTADbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var role = await dbContext.Roles
            .Include(r => r.Auditions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (role is null)
            return new NotFoundObjectResult($"Role {id} does not exist.");
        var unicorn = await dbContext.Unicorns
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (unicorn is null)
            return new NotFoundObjectResult($"Unicorn {unicornId} does not exist.");

        if (role.Auditions.Any(a => a.UnicornId != unicornId && a.IsSuccessful))
            return new ConflictObjectResult("Another unicorn has already been selected for this role.");

        var audition = role.Auditions.FirstOrDefault(a => a.UnicornId == unicornId);
        if (audition is null)
            return new NotFoundObjectResult($"Unicorn {unicornId} has not auditioned for role {id}.");

        audition.IsSuccessful = true;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AcceptedResult();
    }

    private static async Task<IActionResult> Audition(
        int id,
        int unicornId,
        [FromServices] UTADbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var role = await dbContext.Roles
            .Include(r => r.Auditions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (role is null)
            return new NotFoundObjectResult($"Role {id} does not exist.");
        var unicorn = await dbContext.Unicorns
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (unicorn is null)
            return new NotFoundObjectResult($"Unicorn {unicornId} does not exist.");

        if (role.Auditions.Any(a => a.UnicornId != unicornId && a.IsSuccessful))
            return new ConflictObjectResult("Another unicorn has already been selected for this role.");

        if (!role.Auditions.Any(a => a.UnicornId == unicornId))
        {
            role.Auditions.Add(new Audition()
            {
                UnicornId = unicornId,
                Unicorn = unicorn,
                RoleId = id,
                Role = role
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        return new OkResult();
    }

    private static Task<CastingRoleArchiveDto[]> GetCastingRoles(
        [FromServices] UTADbContext dbContext, CancellationToken cancellationToken = default)
        => dbContext.Roles.Select(r => new CastingRoleArchiveDto(
            r.Id,
            r.Name,
            r.When
        )).ToArrayAsync(cancellationToken);

    private static async Task<CastingRoleDetailsDto?> GetCastingRole(
        int id,
        [FromServices] UTADbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Roles.FindAsync(id, cancellationToken);
        if (entity is null) return null;
        return new CastingRoleDetailsDto(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Location,
            entity.Pay,
            entity.When
        );
    }
}