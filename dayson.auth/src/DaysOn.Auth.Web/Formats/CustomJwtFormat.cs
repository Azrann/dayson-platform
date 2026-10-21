using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Thinktecture.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace DaysOn.Auth.Web.Formats
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "REPLACE_WITH_YOUR_VALUE";

        private static readonly byte[] Secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["JWT.Secret"]);
        private readonly string _issuer;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            //var signingKey = new HmacSigningCredentials(Secret);
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Secret);
            var signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(_issuer, AudiencePropertyKey, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey));
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}