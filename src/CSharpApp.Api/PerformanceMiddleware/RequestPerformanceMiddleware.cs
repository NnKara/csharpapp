using System.Diagnostics;

namespace CSharpApp.Api.PerformanceMiddleware
{
    public sealed class RequestPerformanceMiddleware : IMiddleware
    {

        private readonly ILogger<RequestPerformanceMiddleware> _logger;
        public RequestPerformanceMiddleware(ILogger<RequestPerformanceMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                await next(context);
            }
            finally
            {
                stopWatch.Stop();

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path.Value,
                    context.Response.StatusCode,
                    stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
