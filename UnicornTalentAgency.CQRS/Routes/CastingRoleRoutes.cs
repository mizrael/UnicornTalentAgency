using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;

namespace UnicornTalentAgency.CQRS.Routes;

public static class CastingRoleRoutes
{
    public static WebApplication MapCastingRoleRoutes(this WebApplication app)
    {
        var api = app.MapGroup("/api/roles");

        api.MapGet("/", GetCastingRoles);

        api.MapGet("/{id:int}", GetCastingRole);

        api.MapPost("/{id:int}/audition/{unicornId:int}", Audition);

        api.MapPost("/{id:int}/audition/{unicornId:int}/select", SelectUnicorn);

        return app;
    }

    private static async Task<Results<NotFound<string>, BadRequest<string>, Ok>> Audition(
        int id,
        int unicornId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static async Task<Results<NotFound<string>, BadRequest<string>, Ok>> SelectUnicorn(
        int id,
        int unicornId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static async Task<IEnumerable> GetCastingRoles(        
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static async Task GetCastingRole(
        int id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}