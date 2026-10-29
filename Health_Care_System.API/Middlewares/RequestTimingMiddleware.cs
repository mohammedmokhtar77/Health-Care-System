using System.Diagnostics;

namespace Health_Care_System.API.Middlewares;

public class RequestTimingMiddleware(
    RequestDelegate  next 
    , ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = Stopwatch.StartNew();
        Exception? failure = null;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            failure = ex;
            throw;
        }
        finally
        {
            stopWatch.Stop();

            if (failure is null)
            {
                logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms.",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopWatch.ElapsedMilliseconds
                ); 
            }
            else
            {
                logger.LogError(
                    "HTTP {Method} {Path} failed after {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopWatch.ElapsedMilliseconds
                ); 
            }
        }
    }
}