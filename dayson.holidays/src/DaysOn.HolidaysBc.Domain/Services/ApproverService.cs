using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("ApproverService", LifestyleType.Transient)]
    public class ApproverService : IApproverService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeesByApproverService _employeesByApproverService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ApproverService(IEmployeeService employeeService, IEmployeesByApproverService employeesByApproverService)
        {
            _employeeService = employeeService;
            _employeesByApproverService = employeesByApproverService;
        }
        
        public bool AddApproverToAnEmployee(Guid companyId ,Guid approverId, Guid employeeId)
        {
            var employee = _employeeService.GetEmployee(employeeId, companyId);

            if (employee == null)
            {
                _logger.Warn($"[AddEmployeeToApprover] employee {employeeId} not found on company {companyId}");
                return false;
            }

            if (employee.Approver != null)
            {
                RemoveApproverToAnEmployee(companyId, employeeId);
            }

            _employeeService.SetApproverForEmployee(employeeId, approverId);
            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId, employee.Name);

            return true;
        }

        public bool RemoveApproverToAnEmployee(Guid companyId, Guid employeeId)
        {
            var employee = _employeeService.GetEmployeeWith(employeeId, "Approver");

            if (employee == null)
            {
                _logger.Warn($"[AddEmployeeToApprover] employee {employeeId} not found on company {companyId}");
                return false;
            }

            if (employee.Approver != null)
            {
                Guid approverId = employee.Approver.Id;
                _employeeService.RemoveApproverForEmployee(employeeId);
                _employeesByApproverService.RemoveEmployeeFromApprover(approverId, employeeId);
            }

            return true;
        }

        public List<IdNameDto> GetApprovers(Guid companyId)
        {
            return _employeeService.GetApprovers(companyId);
        }

        public bool AddApprover(Guid companyId, Guid approverId)
        {
            var approver = _employeeService.GetEmployee(approverId, companyId);

            if (approver == null)
            {
                _logger.Warn($"[AddApprover] approver {approverId} not found on company {companyId}");
                return false;
            }

            _employeeService.AddApprover(approverId);
            return true;
        }

        public bool RemoveApprover(Guid companyId, Guid approverId)
        {
            var approver = _employeeService.GetEmployee(approverId, companyId);

            if (approver == null)
            {
                _logger.Warn($"[RemoveApprover] approver {approverId} not found on company {companyId}");
                return false;
            }

            _employeeService.RemoveApprover(approverId);
            _employeesByApproverService.RemoveApprover(approverId);

            return true;
        }

        public EmployeesByApprover GetEmployeesForApprover(Guid companyId, Guid approverId)
        {
            var approver = _employeeService.GetEmployee(approverId, companyId);

            if (approver == null)
            {
                _logger.Warn($"[RemoveApprover] approver {approverId} not found on company {companyId}");
                return new EmployeesByApprover();
            }

            return _employeesByApproverService.GetEmployeesForApprover(approverId);
        }
    }
}
