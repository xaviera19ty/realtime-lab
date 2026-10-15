using Serilog.Context;

namespace RealtimeLab.Api.Infrastructure;

public static class RequestIdHeader
{
    public const string Name = "X-Request-Id";
}

/// <summary>每個請求一個 request id：寫進回應 header 與 log，轉給 Go 時一起帶過去。</summary>
public class RequestIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var id = context.Request.Headers[RequestIdHeader.Name].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(id))
        {
            id = Guid.NewGuid().ToString("N")[..16];
        }

        context.TraceIdentifier = id;
        context.Response.Headers[RequestIdHeader.Name] = id;
        using (LogContext.PushProperty("RequestId", id))
        {
            await next(context);
        }
    }
}

/// <summary>HttpClient 呼叫 core 時自動帶上目前請求的 request id。</summary>
public class RequestIdHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var id = accessor.HttpContext?.TraceIdentifier;
        if (!string.IsNullOrEmpty(id))
        {
            request.Headers.TryAddWithoutValidation(RequestIdHeader.Name, id);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
