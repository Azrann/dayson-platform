using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.Auth.Domain.Services
{
    [CastleComponent]
    public class ResetPasswordService : IResetPasswordService
    {
        public IRepository<ResetPasswordCode> ResetPasswordRepository { get; set; }
        public IUserManagerWrapper UserManagerWrapper { get; set; }
        public IUserStoreWrapper UserStoreWrapper { get; set; }
        public IUserManagerFactory UserManagerFactory { get; set; }

        private readonly INonActiveUsersService _nonActiveUsersService;

        public ResetPasswordService(INonActiveUsersService nonActiveUsersService)
        {
            _nonActiveUsersService = nonActiveUsersService;
        }

        public async Task CreateResetPasswordCodeAsync(Guid code, string email)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                var user = await UserManagerWrapper.FindByEmailAsync(email, userManager);

                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.EmailCouldNotBeFound, email));
                }

                ResetPasswordRepository.Add(new ResetPasswordCode(code, user.Id.ToString()));
                ResetPasswordRepository.Save();
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordCommand command)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                var passwordToken = ResetPasswordRepository.GetAll(x => x.Id == command.ResetPasswordToken).FirstOrDefault();

                if (passwordToken == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.InvalidResetPasswordToken, command.ResetPasswordToken));
                }

                if (passwordToken.ExpiryDate < DateTime.UtcNow)
                {
                    throw new ArgumentException(string.Format(ApiErrors.ResetPasswordTokenHasExpired, command.ResetPasswordToken));
                }

                User user = await UserManagerWrapper.FindByIdAsync(passwordToken.UserId, userManager);

                if (user == null)
                {
                    throw new Exception(string.Format(ApiErrors.UserCouldNotBeFound, passwordToken.UserId));
                }

                string hashedNewPassword = UserManagerWrapper.HashPassword(command.Password, userManager);

                await UserStoreWrapper.ResetPasswordAsync(user, hashedNewPassword);
                
                user.EmailConfirmed = true;

                await UserManagerWrapper.UpdateAsync(user, userManager);

                _nonActiveUsersService.SetUserActive(user.CompanyId.ToString(), user.Id);
            }
        }

        public async Task ResetToRandomPassword(string email)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                User user = await UserManagerWrapper.FindByEmailAsync(email, userManager);

                if (user == null)
                {
                    throw new Exception(string.Format(ApiErrors.UserCouldNotBeFound, email));
                }

                string hashedNewPassword = UserManagerWrapper.HashPassword(Guid.NewGuid().ToString("d").Substring(1, 8), userManager);

                await UserStoreWrapper.ResetPasswordAsync(user, hashedNewPassword);

                await UserManagerWrapper.UpdateAsync(user, userManager);
            }
        }
    }
}
