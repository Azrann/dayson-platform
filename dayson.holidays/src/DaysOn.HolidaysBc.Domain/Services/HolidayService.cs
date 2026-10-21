using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Events;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("HolidayService", LifestyleType.Transient)]
    public class HolidayService : IHolidayService
    {
        private readonly IRepository<HolidayRequest> _holidayRequestRepository;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly IHolidayRepo _holidayRepo;
        private readonly IApproverService _approverService;
        private readonly IEmployeeRepo _employeeRepo;

        public HolidayService(IRepository<HolidayRequest> holidayRequestRepository, IRepository<Employee> repositoryEmployee, 
            IHolidayRepo holidayRepo, IApproverService approverService, IEmployeeRepo employeeRepo)
        {
            _holidayRequestRepository = holidayRequestRepository;
            _repositoryEmployee = repositoryEmployee;
            _holidayRepo = holidayRepo;
            _approverService = approverService;
            _employeeRepo = employeeRepo;

            _repositoryEmployee.Context = _holidayRequestRepository.Context;
        }

        public async Task AddHolidayRequest(HolidayRequest holidayRequest, Guid employeeId)
        {
            if (holidayRequest == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "HolidayRequest"));
            }

            Employee employee = _repositoryEmployee.GetAll(x => x.Id == employeeId).FirstOrDefault();
            
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            holidayRequest.Status = holidayRequest.Status;
            holidayRequest.EmployeeId = employeeId;
            holidayRequest.SetYear(employee.YearStartDay);
            _holidayRequestRepository.Add(holidayRequest);
            _holidayRequestRepository.Save();

            await DomainEvents.Raise(new UpdateCalculationsEvent(employeeId, holidayRequest.Year));
        }

        public async Task RemoveHolidayRequest(Guid holidayRequestId, Guid employeeId)
        {
            var holidayRequest = _holidayRequestRepository.GetAll(x => x.Id == holidayRequestId && x.EmployeeId == employeeId).FirstOrDefault();

            if (holidayRequest == null)
            {
                return;
            }

            int year = holidayRequest.Year;
            _holidayRequestRepository.Delete(holidayRequest);
            _holidayRequestRepository.Save();

            await DomainEvents.Raise(new UpdateCalculationsEvent(employeeId, year));
        }

        public async Task CancelHolidayRequest(Guid holidayRequestId, Guid employeeId)
        {
            var holidayRequest = _holidayRequestRepository.GetAll(x => x.Id == holidayRequestId && x.EmployeeId == employeeId).FirstOrDefault();

            if (holidayRequest == null)
            {
                throw new ArgumentException(string.Format(Error.HolidayRequestNotFound, holidayRequestId));
            }

            holidayRequest.Cancelled();
            _holidayRequestRepository.Save();

            await DomainEvents.Raise(new UpdateCalculationsEvent(employeeId, holidayRequest.Year));
        }

        public async Task RejectHolidayRequest(Guid holidayRequestId, Guid employeeId)
        {
            var holidayRequest = _holidayRequestRepository.GetAll(x => x.Id == holidayRequestId && x.EmployeeId == employeeId).FirstOrDefault();

            if (holidayRequest == null)
            {
                throw new ArgumentException(string.Format(Error.HolidayRequestNotFound, holidayRequestId));
            }

            holidayRequest.Rejected();
            _holidayRequestRepository.Save();

            await DomainEvents.Raise(new UpdateCalculationsEvent(employeeId, holidayRequest.Year));
        }

        public async Task ApprovedHolidayRequest(Guid holidayRequestId, Guid employeeId)
        {
            var holidayRequest = _holidayRequestRepository.GetAll(x => x.Id == holidayRequestId && x.EmployeeId == employeeId).FirstOrDefault();

            if (holidayRequest == null)
            {
                throw new ArgumentException(string.Format(Error.HolidayRequestNotFound, holidayRequestId));
            }

            holidayRequest.Approved();
            _holidayRequestRepository.Save();

            await DomainEvents.Raise(new UpdateCalculationsEvent(employeeId, holidayRequest.Year));
        }

        public async Task<HolidayRequest> GetHolidayRequest(Guid holidayRequestId, Guid employeeId)
        {
            var holidayRequest = await Task.Run(() =>_holidayRequestRepository.GetAll(x => x.Id == holidayRequestId && x.EmployeeId == employeeId).FirstOrDefault());

            if (holidayRequest == null)
            {
                throw new ArgumentException(string.Format(Error.HolidayRequestNotFound, holidayRequestId));
            }

            return holidayRequest;
        }

        public async Task<List<HolidayRequest>> GetAllHolidayRequests(Guid employeeId)
        {
            return await Task.Run(() =>_holidayRequestRepository.GetAll(x => x.EmployeeId == employeeId).ToList());
        }

        public async Task<List<HolidayRequest>> GetHolidaysRequestsByYear(Guid employeeId, int year)
        {
            return await Task.Run(() => _holidayRequestRepository.GetAll(x => x.EmployeeId == employeeId).Where(x => x.Year == year).ToList());
        }

        public void RemoveAllHolidayRequests(Guid employeeId)
        {
            _holidayRequestRepository.Delete(x => x.EmployeeId == employeeId);
        }

        public decimal GetSumHolidaysPerYear(Guid employeeId, int year)
        {
            return _holidayRequestRepository.GetAll(x => x.EmployeeId == employeeId && x.Status != Status.Canceled && x.Status != Status.Rejected).Where(x => x.Year == year).Sum(x => x.NumberOfDays);
        }

        public async Task<HolidayRequestDto> AdminChangeHolidayStatus(Guid requestHolidayId, Status requestStatus, Guid requesterId,
            Guid companyId)
        {
            Employee requester = _employeeRepo.GetEmployeeDetails(requesterId);
            if (requester.CompanyId != companyId)
            {
                throw new Exception(string.Format(Error.EmployeeNotFoundInCompany,requesterId,companyId));
            }

            _holidayRepo.ChangeStatus(requestHolidayId, requestStatus);
            var result = _holidayRepo.GetHoliday(requestHolidayId);
            var empId = _holidayRepo.GetHoliday(requestHolidayId).EmployeeId;
            await DomainEvents.Raise(new UpdateCalculationsEvent(empId, result.Year));
            return result;
        }


        public async Task<HolidayRequestDto> ChangeHolidayStatus(Guid requestHolidayId, Status requestStatus, Guid requesterId, Guid companyId)
        {
            Employee requester = _employeeRepo.GetEmployeeDetails(requesterId);
            if (requester.CompanyId != companyId)
            {
                throw new Exception(string.Format(Error.EmployeeNotFoundInCompany, requesterId, companyId));
            }

            var empId = _holidayRepo.GetHoliday(requestHolidayId).EmployeeId;
            if (requester.IsApprover)
            {
                  if (_approverService.GetEmployeesForApprover(companyId, empId).Employees.Any(x => x.Id == requesterId))
                  {
                     _holidayRepo.ChangeStatus(requestHolidayId, requestStatus);
                      var result = _holidayRepo.GetHoliday(requestHolidayId);
                      await DomainEvents.Raise(new UpdateCalculationsEvent(empId, result.Year));
                      return result;

                }
            }
           
            if (requesterId == empId && requestStatus == Status.Canceled)
            {
                _holidayRepo.ChangeStatus(requestHolidayId, requestStatus);
                var result = _holidayRepo.GetHoliday(requestHolidayId);
                await DomainEvents.Raise(new UpdateCalculationsEvent(empId, result.Year));
                return result;
            }
            return null;
        }
    }
}
