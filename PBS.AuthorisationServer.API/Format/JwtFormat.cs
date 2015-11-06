using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using PBS.AuthorisationServer.API.Common;
using Thinktecture.IdentityModel.Tokens;

namespace PBS.AuthorisationServer.API.Format
{
    public class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;

        public JwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string resourceId = data.Properties.Dictionary.ContainsKey(Constants.ResourcePropertyKey) ? data.Properties.Dictionary[Constants.ResourcePropertyKey] : null;

            if (string.IsNullOrWhiteSpace(resourceId)) throw new InvalidOperationException("AuthenticationTicket.Properties does not include resource");

            var resource = ResourceStore.FindResource(resourceId);

            string symmetricKeyAsBase64 = resource.Base64Secret;

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, resourceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}