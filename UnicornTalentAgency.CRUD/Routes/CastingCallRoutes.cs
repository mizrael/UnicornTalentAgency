using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence;
using UnicornTalentAgency.CRUD.Persistence.Entities;

namespace UnicornTalentAgency.CRUD.Routes;

public static class CastingCallRoutes
{
    public static WebApplication MapCastingCallRoutes(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1/castingcalls");

        api.MapGet("/", GetCastingCalls)
            .WithName(nameof(GetCastingCalls))
            .WithOpenApi();

        api.MapGet("/{id}", GetCastingCall)
            .WithName(nameof(GetCastingCall))
            .WithOpenApi();

        return app;
    }

    private static Task<CastingRoleArchiveDto[]> GetCastingCalls(
        [FromServices] UTADbContext dbContext, CancellationToken cancellationToken = default)
        => dbContext.Roles.Select(r => new CastingRoleArchiveDto(
            r.Id,
            r.Name,
            r.When
        )).ToArrayAsync(cancellationToken);

    private static async Task<CastingRoleDetailsDto?> GetCastingCall(
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
