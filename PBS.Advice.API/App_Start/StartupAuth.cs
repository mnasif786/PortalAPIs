using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace PBS.Advice.API
{
    public partial class Startup
    {
        public void ConfigureOAuth(IAppBuilder app)
        {
            var authorisationServerSettings = (NameValueCollection)ConfigurationManager.GetSection("AuthorisationServerSettings");

            var issuer = authorisationServerSettings["Issuer"];
            var resourceId = authorisationServerSettings["ResourceId"];
            var secret = TextEncodings.Base64Url.Decode(authorisationServerSettings["Base64Secret"]);

            //var issuer = "http://localhost:8090";
            //var clientId = "099153c2625149bc8ecb3e85e03f0022";
            //var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");

            // Api controllers with an [Authorize] attribute will be validated with JWT
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