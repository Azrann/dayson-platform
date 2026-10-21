using System;
using System.Collections.Generic;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        void AddEmployeeWithCompanyRules(Employee employee, Guid companyId);
        Employee GetEmployee(Guid employeeId);
        Employee GetEmployeeWith(Guid employeeId, string approver);
        Employee GetEmployee(Guid employeeId, Guid companyId);
        decimal GetMaxDaysCarryForwardEmployee(Guid employeeId);
        void RemoveEmployee(Guid employeeId);
        YearlyHolidayCalculation CreateCalculationsForEmployeeInYear(Guid employeeId, int year);
        void SetGlobalRules(Guid employeeId, Guid companyId, GlobalRules globalRules);
        void SetApproverForEmployee(Guid employeeId, Guid approverId);
        void RemoveApproverForEmployee(Guid employeeId);
        void RemoveApprover(Guid approverId);
        void AddApprover(Guid approverId);
        List<IdNameDto> GetApprovers(Guid companyId);
    }
}
