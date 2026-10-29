using Microsoft.AspNetCore.Mvc;

namespace Health_Care_System.API.Errors;

public class ExceptionProblemDetailsFactory
{
    public static ProblemDetails CreateProblemDetails(
        Exception exception,
        HttpContext context,
        IHostEnvironment env)
    {
        var problems = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occured.",
            Detail = env.IsDevelopment() ? exception.Message : "Try again later.",
            Instance = context.Request.Path,
        };
        problems.Extensions["traceId"] = context.TraceIdentifier;
        return problems;
    }
}