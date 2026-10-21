using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface IOpenClosedDatesService
    {
        OpenClosedDates GetByYear(Guid companyId, int year);

        OpenClosedDates GetRecurrent(Guid companyId);

        OpenClosedDates GetAllByYear(Guid companyId, int year);

        void AddRecurrentClosedDay(Guid companyId, DateTime recurrentClosed);

        void AddRecurrentClosedHalfDay(Guid companyId, DateTime recurrentClosed);

        void AddClosedDay(Guid companyId, DateTime closedDate);

        void AddClosedHalfDay(Guid companyId, DateTime closedDate);

        void AddRecurrentClosedDays(OpenClosedDates openClosedDates);

        void AddClosedDays(OpenClosedDates openClosedDates, int year);
    }
}