using Microsoft.AspNetCore.Mvc.Testing;

using Microsoft.Extensions.Configuration;

namespace UnicornTalentAgency.Tests;

public class ServerFixture : IAsyncLifetime
{
    private readonly List<IAsyncDisposable> _instances = new();
    private bool _isCQRS = false;

    public Task<HttpClient> CreateClientAsync()
    {
        return _isCQRS ?
            CreateClientCoreAsync<CQRS.Program>() :
            CreateClientCoreAsync<CRUD.Program>();
    }

    private async Task<HttpClient> CreateClientCoreAsync<T>()
        where T : class
    {
        var application = await CreateServer<T>();

        return application.CreateClient();
    }

    private async Task<WebApplicationFactory<T>> CreateServer<T>()
        where T : class
    {
        var application = new WebApplicationFactory<T>();
        _instances.Add(application);

        if(_isCQRS)
            await CQRSDbSeeder.SeedAsync(application.Services);
        else
            await CrudDbSeeder.SeedAsync(application.Services);

        return application;
    }

    public Task InitializeAsync()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", optional: false)
            .Build();

        _isCQRS = config.GetValue<bool>("IsCQRS");

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Task.WhenAll(_instances.Select(i => i.DisposeAsync().AsTask()));

        _instances.Clear();
    }
}
