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
    public class CheckAddManagerRoleEventHandler : IAsyncEventHandler<CheckAddManagerRoleEvent>
    {
        private readonly IUserManagerWrapper _userManager;
        private readonly IUserManagerFactory _userManagerFactory;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CheckAddManagerRoleEventHandler(IUserManagerWrapper userManager, IUserManagerFactory userManagerFactory)
        {
            _userManager = userManager;
            _userManagerFactory = userManagerFactory;
        }

        public async Task Handle(CheckAddManagerRoleEvent domainEvent)
        {
            _logger.LogInf($"CheckAddManagerRoleEventHandler called with id={domainEvent.UserId}.");

            IdentityResult result = null;
            using (var userManager = _userManagerFactory.CreateUserManager())
            {
                var roles = await _userManager.GetRolesAsync(domainEvent.UserId.ToString(), userManager);

                if (roles.All(x => x != "Manager"))
                {
                    result = await _userManager.AddToRoleAsync(domainEvent.UserId.ToString(), "Manager", userManager);
                }
            }

            if (result == null || result.Succeeded)
            {
                _logger.LogInf($"[CheckAddManagerRoleEventHandler] Model=[{JsonConvertExt.SerializeObject(domainEvent)}] completed.");
            }
            else
            {
                _logger.LogErr($"[CheckAddManagerRoleEventHandler] Model=[{JsonConvertExt.SerializeObject(domainEvent)}] Errors=[{JsonConvertExt.SerializeObject(result.Errors)}]");
            }
        }
    }
}
