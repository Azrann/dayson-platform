using System;
using System.Collections.Generic;

namespace DaysOn.Auth.Domain.Services
{
    public interface INonActiveUsersService
    {
        void SetUserNonActive(string companyId, string employeeId);

        void SetUserActive(string companyId, string employeeId);

        List<Guid> GetInactiveUsers(string companyId);
    }
}