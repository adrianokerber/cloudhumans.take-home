using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CloudHumans.ClaudIA.Shared
{
    public sealed class HttpGlobalExceptionHandler : IExceptionHandler
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionHandler> _logger;
        private readonly HttpResponseFactory _httpResponseFactory;

        public HttpGlobalExceptionHandler(IWebHostEnvironment env, ILogger<HttpGlobalExceptionHandler> logger, HttpResponseFactory httpResponseFactory)
        {
            _env = env;
            _logger = logger;
            _httpResponseFactory = httpResponseFactory;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred: {Message}",
                exception.Message);

            var errorResult = _httpResponseFactory.CreateErrorWith500(_env.IsDevelopment() ? exception.ToString() : "An error occurred, try again later.");
            await errorResult.ExecuteAsync(httpContext);

            return true;
        }
    }
}
