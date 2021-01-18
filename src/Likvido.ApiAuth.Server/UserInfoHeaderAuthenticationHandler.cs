using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Likvido.ApiAuth.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Likvido.ApiAuth.Server
{
    public class UserInfoHeaderAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    {
        public UserInfoHeaderOptions UserInfoHeaderOptions { get; } = new UserInfoHeaderOptions();
    }

    public class UserInfoHeaderAuthenticationHandler
        : AuthenticationHandler<UserInfoHeaderAuthenticationSchemeOptions>
    {
        private readonly IOptionsMonitor<UserInfoHeaderAuthenticationSchemeOptions> _options;
        private readonly ILoggerFactory _logger;

        public UserInfoHeaderAuthenticationHandler(
            IOptionsMonitor<UserInfoHeaderAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options;
            _logger = logger;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            ClaimsIdentity claimsIdentity;

            var headerName = _options.CurrentValue.UserInfoHeaderOptions.HeaderName;
            if (!Request.Headers.ContainsKey(headerName))
            {
                return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
            }

            var token = Request.Headers[headerName].ToString();

            try
            {
                claimsIdentity = UserInfoHeaderSerializer.Deserialize(token);
            }
            catch (Exception ex)
            {
                var logger = _logger.CreateLogger<UserInfoHeaderAuthenticationHandler>();
                logger.UserInfoHeaderParsingExceptionOccurred(ex, new Dictionary<string, object>
                {
                    ["Url"] = Request.GetDisplayUrl(),
                    ["UserInfo"] = token,
                    ["HeaderName"] = headerName
                });
                return Task.FromResult(AuthenticateResult.Fail("Header deserialization error"));
            }

            if (claimsIdentity != null)
            {
                // generate AuthenticationTicket from the Identity
                // and current authentication scheme
                var ticket = new AuthenticationTicket(
                    Context.User.InsertToAuthenticatedIdentities(claimsIdentity), Scheme.Name);

                // pass on the ticket to the middleware
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
        }
    }
}
