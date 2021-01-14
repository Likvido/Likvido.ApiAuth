using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Likvido.ApiAuth.Server
{
    internal static class IPrincipalExtensions
    {
        public static ClaimsPrincipal InsertToAuthenticatedIdentities(this ClaimsPrincipal? principal, ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException(nameof(claimsIdentity));
            }

            List<ClaimsIdentity> identities = new List<ClaimsIdentity>();
            if (principal != null)
            {
                identities.AddRange(principal.Identities.Where(i => i.IsAuthenticated));
            }
            identities.Insert(0, claimsIdentity);
            return new ClaimsPrincipal(identities);
        }
    }
}
