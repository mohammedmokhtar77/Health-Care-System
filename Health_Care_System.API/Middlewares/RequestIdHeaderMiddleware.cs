using Microsoft.AspNetCore.Authentication;

namespace Health_Care_System.API.Middlewares;

public class RequestIdHeaderMiddleware (ILogger<RequestIdHeaderMiddleware> logger) : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestId = context.TraceIdentifier;
        context.Response.Headers["Request-Id"] = requestId;
        
        logger.LogDebug(
                    "assigned request ID {RequestId} to {Method} {Path}",
                    requestId,
                    context.Request.Method,
                    context.Request.Path
                );
        return next(context);
    }
}