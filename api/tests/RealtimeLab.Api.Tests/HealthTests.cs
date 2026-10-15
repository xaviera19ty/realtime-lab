using System.Net;

namespace RealtimeLab.Api.Tests;

public class HealthTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Healthz_returns_ok_without_touching_dependencies()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/healthz", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_id_is_echoed_back()
    {
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/healthz");
        request.Headers.Add("X-Request-Id", "test-123");

        var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal("test-123", response.Headers.GetValues("X-Request-Id").Single());
    }

    [Fact]
    public async Task System_status_reports_core_unreachable_instead_of_failing()
    {
        var client = factory.CreateClient();

        var json = await client.GetStringAsync("/api/system/status", TestContext.Current.CancellationToken);

        Assert.Contains("\"api\":\"ok\"", json);
        Assert.Contains("core unreachable", json);
    }
}
