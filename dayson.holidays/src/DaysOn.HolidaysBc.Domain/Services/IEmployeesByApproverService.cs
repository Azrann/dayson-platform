using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface IEmployeesByApproverService
    {
        void AddEmployeeToApprover(Guid approverId, Guid employeeId, string name);
        EmployeesByApprover GetEmployeesForApprover(Guid approverId);
        void RemoveApprover(Guid approverId);
        void RemoveEmployeeFromApprover(Guid approverId, Guid employeeId);
    }
}