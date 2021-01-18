using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Likvido.ApiAuth.Common
{
    public static class IPrincipalExtensions
    {
        public static List<Claim> FindByTypes(this IPrincipal principal, IReadOnlyCollection<string> claimsTypes)
        {
            if (claimsTypes?.Count > 0 &&
                principal is ClaimsPrincipal claimsPrincipal)
            {
                return claimsPrincipal.FindAll(c => claimsTypes.Contains(c.Type)).ToList();
            }

            return new List<Claim>();
        }
    }
}
