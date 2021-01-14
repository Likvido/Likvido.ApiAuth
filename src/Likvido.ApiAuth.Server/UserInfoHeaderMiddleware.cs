using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Likvido.ApiAuth.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Likvido.ApiAuth.Server
{
    public class UserInfoHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserInfoHeaderOptions _options;

        public UserInfoHeaderMiddleware(
            RequestDelegate next,
            UserInfoHeaderOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(
            HttpContext httpContext,
            ILogger<UserInfoHeaderOptions> logger)
        {
            if (httpContext.Request.Headers.TryGetValue(_options.HeaderName, out var header))
            {
                try
                {
                    var claimsIdentity = UserInfoHeaderSerializer
                        .Deserialize(header.ToString(), _options.TokenClaims, _options.AuthType);
                    httpContext.User = httpContext.User.InsertToAuthenticatedIdentities(claimsIdentity);
                }
                catch (Exception ex)
                {
                    logger.UserInfoHeaderParsingExceptionOccurred(ex, new Dictionary<string, object>
                    {
                        ["Url"] = httpContext.Request.GetDisplayUrl(),
                        ["UserInfo"] = header.ToString(),
                        ["HeaderName"] = _options.HeaderName
                    });
                    return;
                }
            }

            await _next(httpContext).ConfigureAwait(false);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserInfoMiddlewareExtensions
    {
        //TODO: better options
        public static IApplicationBuilder UseUserInfoMiddleware(
            this IApplicationBuilder builder,
            UserInfoHeaderOptions? options = null)
        {
            return builder.UseMiddleware<UserInfoHeaderMiddleware>(options ?? new UserInfoHeaderOptions());
        }
    }
}
