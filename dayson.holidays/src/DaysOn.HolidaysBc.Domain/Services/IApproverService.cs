using System;
using System.Collections.Generic;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface IApproverService
    {
        bool AddApproverToAnEmployee(Guid companyId, Guid approverId, Guid employeeId);
        bool RemoveApproverToAnEmployee(Guid companyId, Guid employeeId);
        EmployeesByApprover GetEmployeesForApprover(Guid companyId, Guid approverId);
        bool AddApprover(Guid companyId, Guid approverId);
        bool RemoveApprover(Guid companyId, Guid approverId);
        List<IdNameDto> GetApprovers(Guid companyId);
    }
}