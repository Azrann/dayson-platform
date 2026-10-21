using System;
using System.Configuration;
using Castle.Core;
using DaysOn.Auth.Domain.Application;
using Microsoft.AspNet.Identity;
using PubnubApi;

namespace DaysOn.Auth.Domain.Services
{
    [CastleComponent]
    public class UserIdService : IUserIdService
    {
        public IUserManagerWrapper UserManagerWrapper { get; set; }
        public IUserManagerFactory UserManagerFactory { get; set; }

        public string GetUserIdByEmail(string email)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                var user = userManager.FindByEmail(email);
                if (user != null)
                {
                    return user.Id;
                }
            }
            return null;
        }
    }
}
