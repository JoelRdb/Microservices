using Microsoft.AspNetCore.Http;

namespace Common.Logging.Correlation
{
    public class CorrelationIDGeneratorMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _correlationIdHeader = "X-Correlation-Id";

        public CorrelationIDGeneratorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICorrelationIDGenerator correlationIDGenerator)
        {
            await _next(context);
        }
    }
}
