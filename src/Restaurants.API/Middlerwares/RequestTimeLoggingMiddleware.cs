
using System.Diagnostics;

namespace Restaurants.API.Middlerwares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var watch = Stopwatch.StartNew();
        await next.Invoke(context);
        watch.Stop();
        if (watch.ElapsedMilliseconds > 4000)
        {
            logger.LogWarning("Request [{Method}] at [{Path}] took {ElapsedMilliseconds} ms which is longer than expected",
                context.Request.Method,
                context.Request.Path,
                watch.ElapsedMilliseconds);
        }
    }
}
