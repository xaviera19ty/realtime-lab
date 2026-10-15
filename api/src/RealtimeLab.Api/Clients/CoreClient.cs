namespace RealtimeLab.Api.Clients;

/// <summary>
/// 呼叫 Go core 的內部 API。目前手寫；core/openapi.yaml 穩定後改用 NSwag 產生。
/// </summary>
public class CoreClient(HttpClient http)
{
    public async Task<CorePing?> PingAsync(CancellationToken ct = default) =>
        await http.GetFromJsonAsync<CorePing>("/v1/ping", ct);
}

public record CorePing(string Service, string RequestId);
