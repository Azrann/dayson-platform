using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Application;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace DaysOn.Auth.Domain.Events
{
    public class CheckRemoveManagerRoleEventHandler : IAsyncEventHandler<CheckRemoveManagerRoleEvent>
    {
        private readonly IUserManagerWrapper _userManager;
        private readonly IUserManagerFactory _userManagerFactory;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CheckRemoveManagerRoleEventHandler(IUserManagerWrapper userManager, IUserManagerFactory userManagerFactory)
        {
            _userManager = userManager;
            _userManagerFactory = userManagerFactory;
        }

        public async Task Handle(CheckRemoveManagerRoleEvent domainEvent)
        {
            _logger.LogInf($"CheckRemoveManagerRoleEventHandler called with id={domainEvent.UserId}.");

            IdentityResult result = null;
            using (var userManager = _userManagerFactory.CreateUserManager())
            {
                var claims = await _userManager.GetClaimsAsync(domainEvent.UserId.ToString(), userManager);

                if (claims.All(x => x.Type != "ManagerOf"))
                {
                    result = await _userManager.RemoveFromRoleAsync(domainEvent.UserId.ToString(), "Manager", userManager);
                }
            }

            if (result == null || result.Succeeded)
            {
                _logger.LogInf($"[CheckRemoveManagerRoleEventHandler] Model=[{JsonConvertExt.SerializeObject(domainEvent)}] completed.");
            }
            else
            {
                _logger.LogErr($"[CheckRemoveManagerRoleEventHandler] Model=[{JsonConvertExt.SerializeObject(domainEvent)}] Errors=[{JsonConvertExt.SerializeObject(result.Errors)}]");
            }
        }
    }
}
