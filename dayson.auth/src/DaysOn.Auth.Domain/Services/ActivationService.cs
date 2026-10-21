using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.Auth.Domain.Services
{
    [CastleComponent]
    public class ActivationService : IActivationService
    {
        public IUserManagerWrapper UserManagerWrapper { get; set; }

        public IRepository<ActivationCode> ActivationCodeRespository { get; set; }

        public IUserManagerFactory UserManagerFactory { get; set; }

        public async Task ActivateAsync(Guid activationCode)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                ActivationCode code = ActivationCodeRespository.GetAll(x => x.Id == activationCode).FirstOrDefault();
                if (code == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.ActivationCodeCouldNotBeFound, activationCode));
                }

                User user = await UserManagerWrapper.FindByIdAsync(code.UserId.ToString(), userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, code.UserId));
                }

                user.Activate(code);

                var result = await UserManagerWrapper.UpdateAsync(user, userManager);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Format(ApiErrors.UserCouldNotBeActivated, code.UserId));
                }
            }
        }

        public async Task EmailConfirmed(string userId)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                User user = await UserManagerWrapper.FindByIdAsync(userId, userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, userId));
                }

                user.EmailConfirmed = true;

                var result = await UserManagerWrapper.UpdateAsync(user, userManager);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Format(ApiErrors.UserCouldNotBeActivated, userId));
                }
            }
        }

        public Task CreateActivationCodeAsync(ActivationCode code)
        {
            ActivationCodeRespository.Add(code);
            ActivationCodeRespository.Save();

            return Task.FromResult<object>(null);
        }

        public async Task<ActivationCode> RegenerateActivationCodeAsync(Guid userId)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                User user = await UserManagerWrapper.FindByIdAsync(userId.ToString(), userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, userId));
                }
            }

            var activationCode = new ActivationCode(Guid.NewGuid(), userId);

            await CreateActivationCodeAsync(activationCode);

            return activationCode;
        }
    }
}
