using System;
using System.Collections.Generic;
using Castle.Core;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("EmployeesByApproverService", LifestyleType.Transient)]
    public class EmployeesByApproverService : IEmployeesByApproverService
    {
        private readonly INoSqlDb<EmployeesByApprover> _repoCompanyApprovers;

        public EmployeesByApproverService(INoSqlDb<EmployeesByApprover> repoCompanyApprovers)
        {
            _repoCompanyApprovers = repoCompanyApprovers;
        }
        
        public void AddEmployeeToApprover(Guid approverId, Guid employeeId, string name)
        {
            EmployeesByApprover employeesByApprover =
                _repoCompanyApprovers.GetByKey(NoSqlTables.EmployeesByApprover, approverId.ToString()) ?? new EmployeesByApprover();

            if (employeesByApprover.Employees == null)
            {
                employeesByApprover.Employees = new List<IdNameDto>();
            }

            employeesByApprover.Employees.Add(new IdNameDto()
            {
                Id = employeeId,
                Name = name
            });
            _repoCompanyApprovers.AddOrUpdate(NoSqlTables.EmployeesByApprover,approverId.ToString(), employeesByApprover);
        }

        public void RemoveEmployeeFromApprover(Guid approverId, Guid employeeId)
        {
            EmployeesByApprover employeesByApprover =
                _repoCompanyApprovers.GetByKey(NoSqlTables.EmployeesByApprover, approverId.ToString()) ?? new EmployeesByApprover();

            employeesByApprover.Employees.RemoveAll(x => x.Id == employeeId);
            _repoCompanyApprovers.AddOrUpdate(NoSqlTables.EmployeesByApprover, approverId.ToString(), employeesByApprover);
        }

        public void RemoveApprover(Guid approverId)
        {
            _repoCompanyApprovers.Delete(NoSqlTables.EmployeesByApprover,approverId.ToString());
        }

        public EmployeesByApprover GetEmployeesForApprover(Guid approverId)
        {
            return _repoCompanyApprovers.GetByKey(NoSqlTables.EmployeesByApprover, approverId.ToString()) ?? new EmployeesByApprover()
            {
                Employees = new List<IdNameDto>()
            };
        }
    }
}
