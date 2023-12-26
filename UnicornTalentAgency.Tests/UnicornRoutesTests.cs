namespace UnicornTalentAgency.Tests;

public class UnicornRoutesTests : IClassFixture<ServerFixture>
{
    private readonly ServerFixture _fixture;

    public UnicornRoutesTests(ServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetUnicorns_should_return_archive()
    {
        using var client = await _fixture.CreateClientAsync();
        var results = await client.GetFromJsonAsync<IEnumerable<dynamic>>("api/unicorns");
        results.Should().NotBeNullOrEmpty();
    }
}