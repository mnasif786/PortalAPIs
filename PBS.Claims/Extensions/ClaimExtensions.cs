using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PBS.Claims.Extensions
{
    // SGG: This isn't right. Putting these methods in a class means any changes to claim-checking will require all code 
    // to be rebuilt, losing the benefit of the microservice architecture. There should really be a claims microservice,
    // so all logic to claims is "hidden" from other services.
    // However, in the short term we can leave this as we need to revisit when Single Sign On has been agreed with Bright HR 
    public static class ClaimExtensions
    {

        public static bool CanViewJobsForClientId(this IPrincipal user, long ClientID)
        {
            ClaimsPrincipal claimsPrincipal = user as ClaimsPrincipal;
            if (claimsPrincipal == null)
                throw new Exception("Invalid User - Not Claims Prinicpal");

            return claimsPrincipal.HasClaim(PBS.Claims.ClaimTypes.PBSClientId, ClientID.ToString());
        }

        public static bool CanViewDetailsForClientId(this IPrincipal user, long ClientID)
        {
            ClaimsPrincipal claimsPrincipal = user as ClaimsPrincipal;
            if (claimsPrincipal == null)
                throw new Exception("Invalid User - Not Claims Prinicpal");

            return claimsPrincipal.HasClaim(PBS.Claims.ClaimTypes.PBSClientId, ClientID.ToString());
        }

        public static bool CanViewDocsForClientId(this IPrincipal user, long ClientID)
        {
            ClaimsPrincipal claimsPrincipal = user as ClaimsPrincipal;
            if (claimsPrincipal == null)
                throw new Exception("Invalid User - Not Claims Prinicpal");

            return claimsPrincipal.HasClaim(PBS.Claims.ClaimTypes.PBSClientId, ClientID.ToString());
        }
    }
}
