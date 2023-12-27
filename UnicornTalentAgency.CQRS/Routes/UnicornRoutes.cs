using MediatR;
using Microsoft.AspNetCore.Mvc;
using UnicornTalentAgency.CQRS.Read.Queries;

namespace UnicornTalentAgency.CQRS.Routes;

public static class UnicornRoutes
{
    public static WebApplication MapUnicornRoutes(this WebApplication app)
    {
        var api = app.MapGroup("/api/unicorns");

        api.MapGet("/", GetUnicorns);

        api.MapGet("/{id}", GetUnicorn);

        return app;
    }

    private static async Task<IEnumerable<UnicornArchiveItem>> GetUnicorns(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var results = await mediator.Send(new GetUnicornArchiveItem(), cancellationToken);
        return results ?? Enumerable.Empty<UnicornArchiveItem>();
    }

    private static async Task<UnicornDetails?> GetUnicorn(
        int id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetUnicornDetails(id), cancellationToken);
    }
}