using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Likvido.ApiAuth.Common;

namespace Likvido.ApiAuth.Client
{
    public static class WebHeaderCollectionExtensions
    {
        public static WebHeaderCollection SetUserInfoHeader(
            this WebHeaderCollection headerCollection,
            string userId,
            IEnumerable<Claim> claims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            headerCollection.Set(headerName, UserInfoHeaderSerializer.Serialize(userId, claims));
            return headerCollection;
        }

        public static WebHeaderCollection SetUserInfoHeader(
            this WebHeaderCollection headerCollection,
            string userId,
            IEnumerable<Claim> claims,
            TokenClaims tokenClaims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            headerCollection.Set(headerName, UserInfoHeaderSerializer.Serialize(userId, claims, tokenClaims));
            return headerCollection;
        }

        public static WebHeaderCollection SetUserInfoHeader(
            this WebHeaderCollection headerCollection,
            IReadOnlyCollection<Claim> claims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            headerCollection.Set(headerName, UserInfoHeaderSerializer.Serialize(claims));
            return headerCollection;
        }

        public static WebHeaderCollection SetUserInfoHeader(
            this WebHeaderCollection headerCollection,
            IReadOnlyCollection<Claim> claims,
            TokenClaims tokenClaims,
            string headerName = AuthConstants.UserInfoHeader)
        {
            headerCollection.Set(headerName, UserInfoHeaderSerializer.Serialize(claims, tokenClaims));
            return headerCollection;
        }
    }
}
