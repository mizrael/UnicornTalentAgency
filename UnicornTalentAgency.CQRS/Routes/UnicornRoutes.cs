using System.Collections;

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

    private static async Task<IEnumerable> GetUnicorns(        
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static async Task GetUnicorn(
        int id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}