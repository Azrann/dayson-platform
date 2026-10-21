using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Data;
using DaysOn.Common.DataAccess;

namespace DaysOn.Auth.Domain.Services
{
    [CastleComponent]
    public class NonActiveUsersService : INonActiveUsersService
    {
        private readonly INoSqlDb<NonActivatedUsers> _nonActivatedRepo;

        public NonActiveUsersService(INoSqlDb<NonActivatedUsers> nonActivatedRepo)
        {
            _nonActivatedRepo = nonActivatedRepo;
        }

        public void SetUserNonActive(string companyId, string employeeId)
        {
            var nonActiveUsers = _nonActivatedRepo.GetByKey(NoSqlTables.NonActivatedUsers, companyId);
            if (nonActiveUsers?.Users == null)
            {
                nonActiveUsers = new NonActivatedUsers()
                {
                    Users = new List<Guid>()
                };
            }
            nonActiveUsers.Users.Add(new Guid(employeeId));
            _nonActivatedRepo.AddOrUpdate(NoSqlTables.NonActivatedUsers, companyId, nonActiveUsers);
        }

        public void SetUserActive(string companyId, string employeeId)
        {
            var nonActiveUsers = _nonActivatedRepo.GetByKey(NoSqlTables.NonActivatedUsers, companyId);
            if (nonActiveUsers?.Users == null)
            {
                nonActiveUsers = new NonActivatedUsers()
                {
                    Users = new List<Guid>()
                };
            }
            nonActiveUsers.Users.Remove(new Guid(employeeId));
            _nonActivatedRepo.AddOrUpdate(NoSqlTables.NonActivatedUsers, companyId, nonActiveUsers);
        }

        public List<Guid> GetInactiveUsers(string companyId)
        {
            var nonActiveUsers = _nonActivatedRepo.GetByKey(NoSqlTables.NonActivatedUsers, companyId);
            if (nonActiveUsers?.Users == null)
            {
                nonActiveUsers = new NonActivatedUsers()
                {
                    Users = new List<Guid>()
                };
            }
            return nonActiveUsers.Users;
        }
    }
}
