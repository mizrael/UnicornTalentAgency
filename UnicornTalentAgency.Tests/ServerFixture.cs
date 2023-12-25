using Microsoft.AspNetCore.Mvc.Testing;

namespace UnicornTalentAgency.Tests;

public class ServerFixture : IAsyncLifetime
{
    private readonly List<IDisposable> _instances = new();

    public async Task<WebApplicationFactory<Program>> CreateServerAsync()
    {
        var application = new WebApplicationFactory<Program>();
        await DbSeeder.SeedAsync(application.Services);

        _instances.Add(application);
        return application;
    }

    public Task InitializeAsync()
    => Task.CompletedTask;

    public Task DisposeAsync()
    {
        foreach (var instance in _instances)
            instance.Dispose();
        _instances.Clear();

        return Task.CompletedTask;
    }
}
