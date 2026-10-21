using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DaysOn.Auth.Web.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
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

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", $"Invalid client_id '{context.ClientId}'");
                return Task.FromResult<object>(null);
            }
            
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var userManager = new UserManager<User>(new UserStore<User>(new AuthContext())))
            {
                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    context.Rejected();
                    return;
                }

                var userRoles = userManager.GetRoles(user.Id);
                int counter = 0;

                while (userRoles.Count == 0)
                {
                    counter++;
                    await Task.Delay(100);
                    userRoles = userManager.GetRoles(user.Id);
                    if (counter > 20)
                    {
                        context.SetError("no_role", "Could not find role for the user");
                        context.Rejected();
                        return;
                    }
                }

                var oAuthIdentity = await userManager.CreateIdentityAsync(user,
                      context.Options.AuthenticationType);

                var properties = CreateProperties(user.UserName, context.ClientId);

                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
            }
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, IdentityUser user, 
            UserManager<User> userManager)
        {
            var identity = new ClaimsIdentity("JWT");

            var userRoles = userManager.GetRoles(user.Id);
            foreach (var role in userRoles)
            {
                identity.AddClaim(new Claim(System.Security.Claims.ClaimTypes.Role, role));
            }

            return identity;
        }

        public static AuthenticationProperties CreateProperties(string userName, string clientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "audience", string.IsNullOrEmpty(clientId) ? string.Empty : clientId }
            };
            return new AuthenticationProperties(data);
        }
    }
}