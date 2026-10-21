using Castle.Core;
using DaysOn.Auth.Domain.Entities;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Claims;
using log4net;
using System.Linq;

namespace DaysOn.Auth.Domain.Application
{
    [CastleComponent]
    public class TeamService : ITeamService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IUserManagerWrapper UserManagerWrapper { get; set; }
        public IUserManagerFactory UserManagerFactory { get; set; }

        public async Task AddEmployee(Guid userId, Guid teamId)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                User user = await UserManagerWrapper.FindByIdAsync(userId.ToString(), userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, userId));
                }

                var result = await UserManagerWrapper.AddClaimAsync(userId.ToString(), new Claim("Team", teamId.ToString()), userManager);
                
                if (!result.Succeeded)
                {
                    throw new Exception(string.Format(ApiErrors.UserCouldNotBeAssignedToTheTeam, userId, teamId));
                }
            }
        }

        public async Task RemoveTeamFromEmployee(Guid userId)
        {
            using (var userManager = UserManagerFactory.CreateUserManager())
            {
                User user = await UserManagerWrapper.FindByIdAsync(userId.ToString(), userManager);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(ApiErrors.UserCouldNotBeFound, userId));
                }

                var claims = (await UserManagerWrapper.GetClaimsAsync(userId.ToString(), userManager)).Where(x => x.Type == "Team");
                foreach (var claim in claims)
                {
                    var result = await UserManagerWrapper.RemoveClaimAsync(userId.ToString(), claim, userManager);

                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Format(ApiErrors.UserCouldNotBeRemovedFromTheTeam, userId));
                    }
                }
            }
        }
    }
}
