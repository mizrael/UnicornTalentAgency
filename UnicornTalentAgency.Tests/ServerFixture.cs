using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UnicornTalentAgency.Tests;

public class ServerFixture : IAsyncLifetime
{
    private readonly List<IAsyncDisposable> _instances = new();

    public async Task<WebApplicationFactory<Program>> CreateServerAsync()
    {
        var application = new WebApplicationFactory<Program>();

        await DbSeeder.SeedAsync(application.Services);

        _instances.Add(application);
        return application;
    }

    public Task InitializeAsync()
    => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await Task.WhenAll(_instances.Select(i => i.DisposeAsync().AsTask()));

        _instances.Clear();
    }
}
