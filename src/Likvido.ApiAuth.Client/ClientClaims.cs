using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Likvido.ApiAuth.Client
{
    public static class ClientClaims
    {
        public static List<string> Default =>
            new List<string>
            {
                ClaimTypes.Role,
                ClaimTypes.Name,
                ClaimTypes.NameIdentifier,
                ClaimTypes.Email
            };

        public static List<string> GetClaimTypes(params string[] extraClaims)
        {
            if(extraClaims?.Any() == true)
            {
                return Default.Union(extraClaims).ToList();
            }

            return Default;
        }
    }
}
