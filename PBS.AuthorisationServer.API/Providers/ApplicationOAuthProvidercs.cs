using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using PBS.AuthorisationServer.API.Models;
using Claim = System.Security.Claims.Claim;
using ClaimTypes = System.Security.Claims.ClaimTypes;
using Constants = PBS.AuthorisationServer.API.Common.Constants;

namespace PBS.AuthorisationServer.API.Providers
{
    public class ApplicationOAuthProvidercs : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            var resource = ResourceStore.FindResource(context.ClientId);

            if (resource == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //DB checks against membership system 
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The email or password is incorrect");
                return;
            }

            
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));


            foreach (var claims in user.Claims)
            {
                identity.AddClaim(new Claim(claims.ClaimType, claims.ClaimValue));
            }
            
            
            var properties = CreateProperties(Constants.ResourcePropertyKey , context.ClientId ?? string.Empty);

            var ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);
        }

        public static AuthenticationProperties CreateProperties(string key, string value)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { key, value }
            };
            return new AuthenticationProperties(data);
        }
    }
}