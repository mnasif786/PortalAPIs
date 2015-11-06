using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace PBS.Portal.API
{
    public partial class Startup
    {
        public void ConfigureOAuth(IAppBuilder app)
        {
            var authorisationServerSettings = (NameValueCollection)ConfigurationManager.GetSection("AuthorisationServerSettings");

            var issuer = authorisationServerSettings["Issuer"];
            var resourceId = authorisationServerSettings["ResourceId"];
            var secret = TextEncodings.Base64Url.Decode(authorisationServerSettings["Base64Secret"]);
            
            //Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { resourceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    }
                });
        }
    }
}