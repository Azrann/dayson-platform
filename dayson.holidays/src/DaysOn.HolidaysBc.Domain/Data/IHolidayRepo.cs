using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Data
{
    public interface IHolidayRepo
    {
        HolidayRequestDto GetHoliday(Guid id);
        void AddHoliday(Guid id, DateTime from, DateTime to, Status status, decimal numberOfDays, string notes,
            Guid employeeId, int year);
        void ChangeStatus(Guid id, Status status);
    }
}