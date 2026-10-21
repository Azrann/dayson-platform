using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Events;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Utilities.Extension;
using log4net;

namespace DaysOn.Auth.Domain.Application
{
    [CastleComponent]
    public class AuthService : IAuthService
    {
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IUserManagerWrapper _userManagerWrapper;
        private readonly IUserManagerFactory _userManagerFactory;
        private readonly INonActiveUsersService _nonActiveUsersService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AuthService(
            IUserManagerWrapper userManagerWrapper,
            IResetPasswordService resetPasswordService,
            IUserManagerFactory userManagerFactory,
            INonActiveUsersService nonActiveUsersService)
        {
            _resetPasswordService = resetPasswordService;
            _userManagerWrapper = userManagerWrapper;
            _userManagerFactory = userManagerFactory;
            _nonActiveUsersService = nonActiveUsersService;
        }

        public async Task<RegisterUserCommandResult> RegisterUserAsync(string email, string password, string companyId = "", string roleName = "User", string employeeId = null)
        {
            using (var userManager = _userManagerFactory.CreateUserManager())
            {
                string userId = string.IsNullOrEmpty(employeeId) ? Guid.NewGuid().ToString() : employeeId;

                companyId = string.IsNullOrEmpty(companyId) ? Guid.Empty.ToString() : companyId;
                var user = new User(userId, email, companyId);

                var identityResult = await _userManagerWrapper.CreateAsync(user, password, userManager);

                if (identityResult.Succeeded)
                {
                    _logger.LogInf($"Registered new user with Id={userId}, Email={email}, Username={email}, CompanyId={companyId}");

                    var resetCode = Guid.NewGuid();
                    await _resetPasswordService.CreateResetPasswordCodeAsync(resetCode, email);

                    await DomainEvents.Raise(new UserRegisteredEvent(email, resetCode));

                    if (!string.IsNullOrEmpty(companyId) && companyId != "00000000-0000-0000-0000-000000000000")
                    {
                        _nonActiveUsersService.SetUserNonActive(companyId, userId);

                        identityResult = await _userManagerWrapper.AddClaimAsync(userId, new Claim("Company", companyId), userManager);
                        _logger.LogInf($"Added Claim to user with Id={userId} for Company with Id={companyId}");
                    }

                    if (!string.IsNullOrEmpty(userId))
                    {
                        identityResult = await _userManagerWrapper.AddClaimAsync(userId, new Claim("Employee", userId), userManager);
                        _logger.LogInf($"Added Claim to user with Id={userId}");
                    }
                }

                if (identityResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        identityResult =
                            await _userManagerWrapper.AddToRoleAsync(userId, roleName, userManager);
                    }
                }

                return new RegisterUserCommandResult()
                {
                    Succeeded = identityResult.Succeeded,
                    Errors = identityResult.Errors.Select(x => x),
                    UserId = userId
                };
            }
        }
    }
}
