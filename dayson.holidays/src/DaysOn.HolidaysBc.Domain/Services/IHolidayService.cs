using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface IHolidayService
    {
        Task AddHolidayRequest(HolidayRequest holidayRequest, Guid employeeId);
        Task RemoveHolidayRequest(Guid holidayRequestId, Guid employeeId);
        Task CancelHolidayRequest(Guid holidayRequestId, Guid employeeId);
        Task RejectHolidayRequest(Guid holidayRequestId, Guid employeeId);
        Task ApprovedHolidayRequest(Guid holidayRequestId, Guid employeeId);
        Task<HolidayRequest> GetHolidayRequest(Guid holidayRequestId, Guid employeeId);
        Task<List<HolidayRequest>> GetAllHolidayRequests(Guid employeeId);
        Task<List<HolidayRequest>> GetHolidaysRequestsByYear(Guid employeeId, int year);
        decimal GetSumHolidaysPerYear(Guid employeeId, int year);
        Task<HolidayRequestDto> AdminChangeHolidayStatus(Guid requestHolidayId, Status requestStatus, Guid requesterId, Guid companyId);
        Task<HolidayRequestDto> ChangeHolidayStatus(Guid requestHolidayId, Status requestStatus, Guid requesterId, Guid companyId);
    }
}