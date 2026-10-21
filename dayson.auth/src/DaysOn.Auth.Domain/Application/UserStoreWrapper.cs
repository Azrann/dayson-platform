using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DaysOn.Auth.Domain.Application
{
    [ExcludeFromCodeCoverage]
    [CastleComponent]
    public class UserStoreWrapper : IUserStoreWrapper
    {
        private readonly UserStore<User> _userStore;

        public UserStoreWrapper(IUserManagerFactory userManagerFactory)
        {
            _userStore = userManagerFactory.CreateUserStore();
        }

        public async Task ResetPasswordAsync(User user, string hashedNewPassword)
        {
            try
            {
                await _userStore.SetPasswordHashAsync(user, hashedNewPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}