using Likvido.ApiAuth.Common;

namespace Likvido.ApiAuth.Server
{
    public class UserInfoHeaderOptions
    {
        public TokenClaims? TokenClaims { get; set; }
        public string HeaderName { get; set; } = AuthConstants.UserInfoHeader;
        public string? AuthType { get; set; } = AuthConstants.UserInfoHeader;
    }
}
