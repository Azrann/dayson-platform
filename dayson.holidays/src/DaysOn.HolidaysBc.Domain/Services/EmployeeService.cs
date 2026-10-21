using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;
using Castle.Core;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("EmployeeService", LifestyleType.Transient)]
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly ICompanyService _companyService;

        public EmployeeService(IRepository<Employee> repositoryEmployee, ICompanyService companyService)
        {
            _repositoryEmployee = repositoryEmployee;
            _companyService = companyService;
        }

        public Employee GetEmployee(Guid employeeId)
        {
            List<string> includesList = new List<string>() { "HolidayRequests", "YearlyHolidayCalculations"};
            var employee = _repositoryEmployee.GetAll(emp => emp.Id == employeeId, includesList).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            return employee;
        }

        public Employee GetEmployeeWith(Guid employeeId, string include)
        {
            List<string> includesList = new List<string>() { include };
            var employee = _repositoryEmployee.GetAll(emp => emp.Id == employeeId, includesList).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            return employee;
        }

        public Employee GetEmployee(Guid employeeId, Guid companyId)
        {
            List<string> includesList = new List<string>() { "HolidayRequests", "YearlyHolidayCalculations" };
            var employee = _repositoryEmployee.GetAll(emp => emp.Id == employeeId && emp.CompanyId == companyId, includesList).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFoundInCompany, employeeId, companyId));
            }

            return employee;
        }

        public void SetGlobalRules(Guid employeeId, Guid companyId, GlobalRules globalRules)
        {
            var employee = _repositoryEmployee.GetAll(emp => emp.Id == employeeId && emp.CompanyId == companyId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFoundInCompany, employeeId, companyId));
            }

            employee.ApplyGlobalRules(globalRules);
            _repositoryEmployee.Save();
        }

        public decimal GetMaxDaysCarryForwardEmployee(Guid employeeId)
        {
            var employee = _repositoryEmployee.GetAll(emp => emp.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            return employee.MaxDaysCarriedOverNextYear;
        }

        public void AddEmployeeWithCompanyRules(Employee employee, Guid companyId)
        {
            GlobalRules globalRules = _companyService.GetGlobalRules(companyId);

            employee.ApplyGlobalRules(globalRules);
            AddEmployee(employee);
        }

        public void AddEmployee(Employee employee)
        {
            _repositoryEmployee.Add(employee);
            _repositoryEmployee.Save();
        }

        public void RemoveEmployee(Guid employeeId)
        {
            Employee employee = _repositoryEmployee.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, employeeId));
            }

            if (employee.IsApprover)
            {
                RemoveApprover(employeeId);
            }

            _repositoryEmployee.Delete(employee);
            _repositoryEmployee.Save();
        }
       
        public YearlyHolidayCalculation CreateCalculationsForEmployeeInYear(Guid employeeId, int year)
        {
            Employee employee = _repositoryEmployee.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, employeeId));
            }

            var result = employee.CreateCalculationInYear(year);
            _repositoryEmployee.Save();

            return result;
        }

        public void SetApproverForEmployee(Guid employeeId, Guid approverId)
        {
            Employee employee = _repositoryEmployee.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, employeeId));
            }

            Employee approver = _repositoryEmployee.GetAll(x => x.Id == approverId).FirstOrDefault();

            if (approverId == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, approverId));
            }

            employee.Approver = approver;
            _repositoryEmployee.Save();
        }

        public void RemoveApproverForEmployee(Guid employeeId)
        {
            Employee employee = _repositoryEmployee.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, employeeId));
            }

            employee.Approver = null;
            _repositoryEmployee.Save();
        }

        public void RemoveApprover(Guid approverId)
        {
            Employee approver = _repositoryEmployee.GetAll(x => x.Id == approverId).FirstOrDefault();

            if (approver == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, approverId));
            }
            approver.IsApprover = false;
            _repositoryEmployee.Save();

            var employees = _repositoryEmployee.GetAll(x => x.Approver != null && x.Approver.Id == approverId);
            foreach (var employee in employees)
            {
                employee.Approver = null;
            }
            _repositoryEmployee.Save();
        }

        public void AddApprover(Guid approverId)
        {
            Employee approver = _repositoryEmployee.GetAll(x => x.Id == approverId).FirstOrDefault();

            if (approver == null)
            {
                throw new Exception(string.Format(Error.EmployeeNotFound, approverId));
            }
            approver.IsApprover = true;
            _repositoryEmployee.Save();
        }


        public List<IdNameDto> GetApprovers(Guid companyId)
        {
            return _repositoryEmployee.GetAll(x => x.IsApprover && x.CompanyId == companyId).Select(x => new IdNameDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
