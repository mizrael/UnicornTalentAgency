using System.Net;

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
        using var client = await _fixture.CreateClientAsync();
        var results = await client.GetFromJsonAsync<IEnumerable<dynamic>>("api/roles");
        results.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Audition_should_return_NotFound_when_role_id_invalid()
    {
        using var client = await _fixture.CreateClientAsync();
        var resp = await client.PostAsync("api/roles/200/audition/1", null);
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Audition_should_return_NotFound_when_unicorn_id_invalid()
    {
        using var client = await _fixture.CreateClientAsync();
        var resp = await client.PostAsync("api/roles/1/audition/200", null);
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Audition_should_return_ok_when_input_valid()
    {
        using var client = await _fixture.CreateClientAsync();
        var resp = await client.PostAsync("api/roles/1/audition/1", null);
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Audition_should_return_ok_when_input_valid_and_request_replayed()
    {
        using var client = await _fixture.CreateClientAsync();
        var resp1 = await client.PostAsync("api/roles/1/audition/1", null);
        resp1.StatusCode.Should().Be(HttpStatusCode.OK);

        var resp2 = await client.PostAsync("api/roles/1/audition/1", null);
        resp2.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SelectUnicorn_should_return_NotFound_when_role_id_invalid()
    {
        using var client = await _fixture.CreateClientAsync();
        var resp = await client.PostAsync("api/roles/200/audition/1/select", null);
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SelectUnicorn_should_return_NotFound_when_unicorn_id_invalid()
    {
        using var client = await _fixture.CreateClientAsync();
        var resp = await client.PostAsync("api/roles/1/audition/200/select", null);
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SelectUnicorn_should_return_ok_when_input_valid()
    {
        using var client = await _fixture.CreateClientAsync();

        var auditionResp = await client.PostAsync("api/roles/1/audition/1", null);
        auditionResp.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var resp = await client.PostAsync("api/roles/1/audition/1/select", null);
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SelectUnicorn_should_return_bad_request_when_another_unicorn_has_been_selected_already()
    {
        using var client = await _fixture.CreateClientAsync();

        var auditionResp = await client.PostAsync("api/roles/1/audition/1", null);
        auditionResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var resp1 = await client.PostAsync("api/roles/1/audition/1/select", null);
        resp1.StatusCode.Should().Be(HttpStatusCode.OK);

        var resp2 = await client.PostAsync("api/roles/1/audition/2/select", null);
        resp2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SelectUnicorn_should_return_bad_request_when_unicorn_not_registered_for_audition()
    {
        using var client = await _fixture.CreateClientAsync();

        var resp = await client.PostAsync("api/roles/1/audition/1/select", null);
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Audition_should_return_bad_request_when_another_unicorn_has_been_selected_already()
    {
        using var client = await _fixture.CreateClientAsync();

        var auditionResp = await client.PostAsync("api/roles/1/audition/1", null);
        auditionResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var resp1 = await client.PostAsync("api/roles/1/audition/1/select", null);
        resp1.StatusCode.Should().Be(HttpStatusCode.OK);

        var resp2 = await client.PostAsync("api/roles/1/audition/2", null);
        resp2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}