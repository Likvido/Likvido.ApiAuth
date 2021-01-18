using System.Collections.Generic;
using System.Security.Claims;

namespace Likvido.ApiAuth.Common
{
    public class TokenClaims
    {
        public static TokenClaims Default => new TokenClaims();

        public const string Roles = "roles";
        public const string Name = "name";
        public const string Id = "sub";
        private static readonly IDictionary<string, string> _claimsToTokenClaims = new Dictionary<string, string>();
        private static readonly IDictionary<string, string> _tokenClaimsToClaims = new Dictionary<string, string>
        {
            [Roles] = ClaimTypes.Role,
            [Name] = ClaimTypes.Name,
            [Id] = ClaimTypes.NameIdentifier,
        };

        static TokenClaims()
        {
            foreach (var kvp in _tokenClaimsToClaims)
            {
                _claimsToTokenClaims.Add(kvp.Value, kvp.Key);
            }
        }

        private readonly IDictionary<string, string> TokenClaimsToClaims = new Dictionary<string, string>();
        private readonly IDictionary<string, string> ClaimsToTokenClaims = new Dictionary<string, string>();

        public TokenClaims()
        {
            foreach (var kvp in _tokenClaimsToClaims)
            {
                TokenClaimsToClaims.Add(kvp.Key, kvp.Value);
                ClaimsToTokenClaims.Add(kvp.Value, kvp.Key);
            }
        }

        public void Clear()
        {
            TokenClaimsToClaims.Clear();
            ClaimsToTokenClaims.Clear();
        }

        public void AddTokenClaimToClaim(string tokenClaim, string claim)
        {
            RemoveTokenClaim(tokenClaim);
            RemoveClaim(claim);
            TokenClaimsToClaims.Add(tokenClaim, claim);
            ClaimsToTokenClaims.Add(claim, tokenClaim);
        }

        public void RemoveTokenClaim(string tokenClaim)
        {
            if (TokenClaimsToClaims.TryGetValue(tokenClaim, out var claim))
            {
                TokenClaimsToClaims.Remove(tokenClaim);
                ClaimsToTokenClaims.Remove(claim);
            }
        }

        public void RemoveClaim(string claim)
        {
            if (ClaimsToTokenClaims.TryGetValue(claim, out var tokenClaim))
            {
                ClaimsToTokenClaims.Remove(claim);
                TokenClaimsToClaims.Remove(tokenClaim);
            }
        }

        public string FromTokenClaimToClaim(string tokenClaim)
        {
            if (TokenClaimsToClaims.TryGetValue(tokenClaim, out var claim))
            {
                return claim;
            }

            return tokenClaim;
        }

        public string FromClaimToTokenClaim(string claim)
        {
            if (ClaimsToTokenClaims.TryGetValue(claim, out var tokenClaim))
            {
                return tokenClaim;
            }

            return claim;
        }
    }
}
