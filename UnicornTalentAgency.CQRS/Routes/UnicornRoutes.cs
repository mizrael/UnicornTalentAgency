using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    private static Task<UnicornArchiveDto[]> GetUnicorns(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static async Task<UnicornDetailsDto?> GetUnicorn(
        int id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}