using Microsoft.AspNetCore.Mvc;
using RealtimeLab.Api.Clients;

namespace RealtimeLab.Api.Controllers;

/// <summary>確認 web → api → core 整條路都通；request id 會一路帶到 core。</summary>
[ApiController]
[Route("api/system")]
public class SystemController(CoreClient core, ILogger<SystemController> logger) : ControllerBase
{
    [HttpGet("status")]
    public async Task<IActionResult> Status(CancellationToken ct)
    {
        object coreStatus;
        try
        {
            var ping = await core.PingAsync(ct);
            coreStatus = new { ok = true, ping?.RequestId };
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "core unreachable");
            coreStatus = new { ok = false, error = "core unreachable" };
        }

        return Ok(new { api = "ok", requestId = HttpContext.TraceIdentifier, core = coreStatus });
    }
}
