using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Entities;
using System.Security.Claims;
using DaysOn.Auth.Domain.Events;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.Auth.Domain.Application
{
    [CastleComponent]
    public class ManagerOfTeamService : IManagerOfTeamService
    {
        private readonly IUserManagerWrapper _userManagerWrapper;
        private readonly IUserManagerFactory _userManagerFactory;

        public ManagerOfTeamService(IUserManagerWrapper userManagerWrapper, IUserManagerFactory userManagerFactory)
        {
            _userManagerWrapper = userManagerWrapper;
            _userManagerFactory = userManagerFactory;
        }

        public async Task AddManager(Guid userId, Guid teamId)
        {
            using (var userManager = _userManagerFactory.CreateUserManager())
            {
                User user = await _userManagerWrapper.FindByIdAsync(userId.ToString(), userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, userId));
                }

                var result = await _userManagerWrapper.AddClaimAsync(userId.ToString(), new Claim("ManagerOf", teamId.ToString()), userManager);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Format(ApiErrors.UserCouldNotBeAddedAsAManagerForTheTeam, userId, teamId));
                }
                await DomainEvents.Raise(new CheckAddManagerRoleEvent(userId));
            }
        }

        public async Task RemoveManagerFromTeam(Guid userId, Guid teamId)
        {
            using (var userManager = _userManagerFactory.CreateUserManager())
            {
                User user = await _userManagerWrapper.FindByIdAsync(userId.ToString(), userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, userId));
                }

                var claims = (await _userManagerWrapper.GetClaimsAsync(userId.ToString(), userManager)).Where(x => x.Type == "ManagerOf");
                foreach (var claim in claims)
                {
                    if (claim.Value == teamId.ToString())
                    { 
                        var result = await _userManagerWrapper.RemoveClaimAsync(userId.ToString(), claim, userManager);

                        if (!result.Succeeded)
                        {
                            throw new Exception(string.Format(ApiErrors.UserCouldNotBeRemovedAsManagerFromTheTeam, userId , teamId));
                        }
                        await DomainEvents.Raise(new CheckRemoveManagerRoleEvent(userId));
                    }
                }
            }
        }
    }
}
