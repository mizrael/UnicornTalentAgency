using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
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

        api.MapGet("/", GetCastingRoles)
            .WithName(nameof(GetCastingRoles))
            .WithOpenApi();

        api.MapGet("/{id:int}", GetCastingRole)
            .WithName(nameof(GetCastingRole))
            .WithOpenApi();

        api.MapPost("/{id:int}/audition/{unicornId:int}", Audition)
            .WithName(nameof(Audition))
            .WithOpenApi();

        return app;
    }

    private static async Task<IActionResult> Audition(
        int id,
        int unicornId,
        [FromServices] UTADbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var role = await dbContext.Roles
            .AsNoTracking()
            .Include(r => r.Auditions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (role is null)
            return new NotFoundResult();
        var unicorn = await dbContext.Unicorns
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (unicorn is null)
            return new NotFoundResult();

        if (role.Auditions.Any(a => a.UnicornId == unicornId))
            return new OkResult();

        role.Auditions.Add(new Audition()
        {
            UnicornId = unicornId,
            Unicorn = unicorn,
            RoleId = id,
            Role = role
        });
        await dbContext.SaveChangesAsync(cancellationToken);

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