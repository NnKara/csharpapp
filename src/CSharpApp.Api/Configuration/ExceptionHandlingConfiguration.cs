using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Configuration;

public static class ExceptionHandlingConfiguration
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(inner =>
        {
            inner.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerFeature?.Error;

                var (statusCode, title) = exception switch
                {
                    TaskCanceledException or TimeoutException => (StatusCodes.Status503ServiceUnavailable, "Third-party service is unavailable"),
                    HttpRequestException => (StatusCodes.Status502BadGateway, "Third-party service error"),
                    _ => (StatusCodes.Status500InternalServerError, "Server error")
                };

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = "Request failed.",
                    Instance = exceptionHandlerFeature?.Path
                };

                problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });

        return app;
    }
}

