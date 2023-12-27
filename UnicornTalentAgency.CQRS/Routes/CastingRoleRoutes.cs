using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UnicornTalentAgency.CQRS.Read.Queries;
using UnicornTalentAgency.CQRS.Write.Commands;

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
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await mediator.Send(new ApplyForAudition(unicornId, id), cancellationToken);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return TypedResults.NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return TypedResults.BadRequest(e.Message);
        }

        return TypedResults.Ok();
    }

    private static async Task<Results<NotFound<string>, BadRequest<string>, Ok>> SelectUnicorn(
        int id,
        int unicornId,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await mediator.Send(new SelectUnicornForRole(unicornId, id), cancellationToken);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return TypedResults.NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return TypedResults.BadRequest(e.Message);
        }

        return TypedResults.Ok();
    }

    private static async Task<IEnumerable<CastingRoleArchive>> GetCastingRoles(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    { 
        var results = await mediator.Send(new GetCastingRoleArchive(), cancellationToken);
        return results ?? Enumerable.Empty<CastingRoleArchive>();
    }

    private static async Task<CastingRoleDetails?> GetCastingRole(
        int id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var results = await mediator.Send(new GetCastingRoleDetails(id), cancellationToken);
        return results;
    }
}