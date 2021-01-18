using Likvido.ApiAuth.Common;

namespace Likvido.ApiAuth.Server
{
    public interface IApiKeyAuthOptions
    {
        string HeaderName { get; }
        string GetUserName(string apiKey);
    }

    public abstract class ApiKeyAuthOptionsBase : IApiKeyAuthOptions
    {
        public string HeaderName => AuthConstants.ApiKeyHeader;

        public abstract string GetUserName(string apiKey);
    }
}
