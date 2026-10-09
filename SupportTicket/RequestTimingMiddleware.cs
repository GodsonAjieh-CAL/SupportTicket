using System.Diagnostics;

namespace SupportTicket
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var startTime = Stopwatch.StartNew();
            await _next(context);
            startTime.Stop();
            _logger.LogInformation("{Path} took {ElapsedMs}ms", context.Request.Path, startTime.ElapsedMilliseconds);

        }

    }
}
