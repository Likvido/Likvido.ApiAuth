using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Likvido.ApiAuth.Common
{
    public static class UserInfoHeaderSerializer
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            MaxDepth = 3,
            WriteIndented = false
        };

        private static readonly JsonDocumentOptions DocumentOptions = new JsonDocumentOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
            MaxDepth = 3
        };

        public static ClaimsIdentity Deserialize(string value, TokenClaims tokenClaims = null, string authType = AuthConstants.UserInfoHeader)
        {
            tokenClaims = tokenClaims ?? TokenClaims.Default;
            var rawString = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            var doc = JsonDocument.Parse(rawString, DocumentOptions);
            var claims = new List<Claim>();
            var jsonUserId = doc.RootElement.GetProperty("userId").GetString();
            var jsonClaims = doc.RootElement.GetProperty("claims");
            bool hasId = false;
            foreach (var claim in jsonClaims.EnumerateObject())
            {
                var claimName = tokenClaims.FromTokenClaimToClaim(claim.Name);
                hasId = hasId || claimName == ClaimTypes.NameIdentifier;
                if (claim.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var claimElement in claim.Value.EnumerateArray())
                    {
                        claims.Add(new Claim(claimName, claimElement.GetString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(claimName, claim.Value.GetString()));
                }
            }

            if (!hasId)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, jsonUserId));
            }

            return new ClaimsIdentity(claims, authType);
        }

        public static string Serialize(string userId, IEnumerable<Claim> claims, TokenClaims tokenClaims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("Value muct be provided", userId);
            }

            var idClaim = claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim != null && idClaim.Value != userId)
            {
                throw new ArgumentException($"UserId and \"{ClaimTypes.NameIdentifier}\" value must match", nameof(userId));
            }

            tokenClaims = tokenClaims ?? TokenClaims.Default;

            var userInfo = new ApiUserInfo
            {
                UserId = userId
            };

            var claimGroups = claims.GroupBy(c => c.Type).ToList();
            foreach (var claimGroup in claimGroups)
            {
                var claimName = tokenClaims.FromClaimToTokenClaim(claimGroup.Key);
                if (claimGroup.Count() > 1)
                {
                    userInfo.Claims.Add(claimName, claimGroup.Select(c => c.Value).ToArray());
                }
                else
                {
                    userInfo.Claims.Add(claimName, claimGroup.First().Value);
                }
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userInfo, SerializerOptions)));
        }

        public static string Serialize(IEnumerable<Claim> claims, TokenClaims tokenClaims = null)
        {
            var idClaim = claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(idClaim?.Value))
            {
                throw new ArgumentException($"\"{ClaimTypes.NameIdentifier}\" claim must exist", nameof(claims));
            }

            tokenClaims = tokenClaims ?? TokenClaims.Default;
            var userInfo = new ApiUserInfo
            {
                UserId = idClaim.Value
            };

            var claimGroups = claims.GroupBy(c => c.Type).ToList();
            foreach (var claimGroup in claimGroups)
            {
                var claimName = tokenClaims.FromClaimToTokenClaim(claimGroup.Key);
                if (claimGroup.Count() > 1)
                {
                    userInfo.Claims.Add(claimName, claimGroup.Select(c => c.Value).ToArray());
                }
                else
                {
                    userInfo.Claims.Add(claimName, claimGroup.First().Value);
                }
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userInfo, SerializerOptions)));
        }

        private class ApiUserInfo
        {
            public string UserId { get; set; }

            public Dictionary<string, object> Claims { get; } = new Dictionary<string, object>();
        }
    }
}
