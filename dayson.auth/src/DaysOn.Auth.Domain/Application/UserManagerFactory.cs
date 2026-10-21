using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DaysOn.Auth.Domain.Application
{
    [ExcludeFromCodeCoverage]
    [CastleComponent]
    public class UserManagerFactory : IUserManagerFactory
    {
        public UserManager<User> CreateUserManager()
        {
            var userManager = new UserManager<User>(new UserStore<User>(new AuthContext()));

            userManager.UserValidator = new UserValidator<User>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            return userManager;
        }

        public UserStore<User> CreateUserStore()
        {
            return new UserStore<User>(new AuthContext());
        }
    }
}