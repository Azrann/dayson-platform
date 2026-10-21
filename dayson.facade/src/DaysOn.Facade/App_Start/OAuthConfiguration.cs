using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace DaysOn.Facade
{
    public static class OAuthConfiguration
    {
        public static void ConfigureOAuth(this IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["JWT.Issuer"];
            var audience = ConfigurationManager.AppSettings["JWT.Audience"];
            var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["JWT.Secret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, secret), 
                    },
                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnValidateIdentity = context =>
                        {
                            context.Ticket.Identity.AddClaim(new System.Security.Claims.Claim("newCustomClaim", "newValue"));
                            return Task.FromResult<object>(null);
                        }
                    }
                });

        }
    }
}
