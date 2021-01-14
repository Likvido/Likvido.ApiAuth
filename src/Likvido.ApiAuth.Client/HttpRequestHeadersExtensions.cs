using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using Likvido.ApiAuth.Common;

namespace Likvido.ApiAuth.Client
{
    public static class HttpRequestHeadersExtensions
    {
        public static HttpRequestHeaders SetUserInfoHeader(
            this HttpRequestHeaders requestHeaders,
            string userId,
            IEnumerable<Claim> claims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            requestHeaders.Remove(headerName);
            requestHeaders.Add(headerName, UserInfoHeaderSerializer.Serialize(userId, claims));
            return requestHeaders;
        }

        public static HttpRequestHeaders SetUserInfoHeader(
            this HttpRequestHeaders requestHeaders,
            string userId,
            IEnumerable<Claim> claims,
            TokenClaims tokenClaims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            requestHeaders.Remove(headerName);
            requestHeaders.Add(headerName, UserInfoHeaderSerializer.Serialize(userId, claims, tokenClaims));
            return requestHeaders;
        }

        public static HttpRequestHeaders SetUserInfoHeader(
            this HttpRequestHeaders requestHeaders,
            IEnumerable<Claim> claims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            requestHeaders.Remove(headerName);
            requestHeaders.Add(headerName, UserInfoHeaderSerializer.Serialize( claims));
            return requestHeaders;
        }

        public static HttpRequestHeaders SetUserInfoHeader(
            this HttpRequestHeaders requestHeaders,
            IEnumerable<Claim> claims,
            TokenClaims tokenClaims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            requestHeaders.Remove(headerName);
            requestHeaders.Add(headerName, UserInfoHeaderSerializer.Serialize(claims, tokenClaims));
            return requestHeaders;
        }
    }
}
