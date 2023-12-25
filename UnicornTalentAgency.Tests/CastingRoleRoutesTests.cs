namespace UnicornTalentAgency.Tests;

public class CastingRoleRoutesTests : IClassFixture<ServerFixture>
{
    private readonly ServerFixture _fixture;

    public CastingRoleRoutesTests(ServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCastingRoles_should_return_archive()
    {
        using var server = await _fixture.CreateServerAsync();
        using var client = server.CreateClient();
        var results = await client.GetFromJsonAsync<IEnumerable<dynamic>>("api/roles");
        results.Should().NotBeNullOrEmpty();
    }
}