using DaysOn.Functions.BoundedContext.Value;
using System;
using System.Threading.Tasks;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.Functions.BoundedContext.Services
{
    public interface ICreateHolidayService
    {
        Task<ValidateAddHolidaysResponse> Validate(AddHolidayRequestEvent command);
        GetBusinessDaysResponse GetBusinessDays(AddHolidayRequestEvent message, ValidateAddHolidaysResponse validationResponse);
        bool GetNonValidatedSystemRules(AddHolidayRequestEvent message, out BaseResultEvent response, decimal numDays);
        bool GetNonValidatedCompanyRules(AddHolidayRequestEvent message, out BaseResultEvent response);
        bool GetNonValidatedTeamRules(AddHolidayRequestEvent message, out BaseResultEvent response);
        Task<HolidayRequest> CreateHolidayRequest(AddHolidayRequestEvent command, decimal numDays, DateTime yearStartDay);
        BaseResultEvent UpdateCalculation(Guid employeeId, int year);
    }
}