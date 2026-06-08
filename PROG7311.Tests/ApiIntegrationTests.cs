using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace PROG7311.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<PROG7311.API.Program>>
{
    // Integration tests for API
    private readonly WebApplicationFactory<PROG7311.API.Program> _factory;

    public ApiIntegrationTests(WebApplicationFactory<PROG7311.API.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetClients_Returns200Or401()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/api/clients");
        Assert.True(resp.IsSuccessStatusCode || resp.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Register_Login_Workflow()
    {
        var client = _factory.CreateClient();

        var user = new { Username = "testuser" + System.Guid.NewGuid().ToString("N").Substring(0,6), Password = "P@ssword1!", Email = "test@example.com" };
        var regResp = await client.PostAsJsonAsync("/api/auth/register", user);
        Assert.True(regResp.IsSuccessStatusCode);

        var loginResp = await client.PostAsJsonAsync("/api/auth/login", new { Username = user.Username, Password = user.Password });
        Assert.True(loginResp.IsSuccessStatusCode);
        var body = await loginResp.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(body);
    }

    [Fact]
    public async Task GetContracts_Returns200Or401()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/api/contracts");
        Assert.True(resp.IsSuccessStatusCode || resp.StatusCode == System.Net.HttpStatusCode.Unauthorized);
    }
}
