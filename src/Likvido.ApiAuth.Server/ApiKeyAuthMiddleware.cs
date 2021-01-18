using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Likvido.ApiAuth.Server
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthMiddleware> _logger;

        public ApiKeyAuthMiddleware(RequestDelegate next, ILogger<ApiKeyAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        //TODO: better options approach
        public async Task Invoke(HttpContext context, IApiKeyAuthOptions options)
        {
            if (context.Request.Headers.TryGetValue(options.HeaderName, out var apiKey))
            {
                var userName = options.GetUserName(apiKey);
                if (string.IsNullOrWhiteSpace(userName))
                {
                    _logger.ApiKeyAuthorizationFailureOccurred(
                        "Invalid Api Key.",
                        new Dictionary<string, object> { ["HeaderName"] = options.HeaderName });
                    WriteUnauthorized(context);
                    return;
                }

                var identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, userName)
                    },
                    options.HeaderName);

                context.User = new ClaimsPrincipal(identity);

                await _next(context).ConfigureAwait(false);
            }
            else
            {
                _logger.ApiKeyAuthorizationFailureOccurred(
                    "Api Key not provided.",
                    new Dictionary<string, object> { ["HeaderName"] = options.HeaderName });
                WriteUnauthorized(context);
            }
        }

        private static void WriteUnauthorized(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyAuthMiddlewareExtensions
    {
        //TODO: better options
        public static IApplicationBuilder UseApiKeyAuthMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyAuthMiddleware>();
        }
    }
}
