using System;
using Castle.Core;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Data;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("OpenClosedDatesService", LifestyleType.Transient)]
    public class OpenClosedDatesService : IOpenClosedDatesService
    {
        private readonly INoSqlDb<OpenClosedDates> _repositoryOpenClosedDates;

        public OpenClosedDatesService(INoSqlDb<OpenClosedDates> repositoryOpenClosedDates)
        {
            _repositoryOpenClosedDates = repositoryOpenClosedDates;
        }

        public OpenClosedDates GetByYear(Guid companyId, int year)
        {
            var result = _repositoryOpenClosedDates.GetByKey(NoSqlTables.OpenClosedDates, GetKey(companyId, year));
            if (result != null)
            {
                return result;
            }
            return new OpenClosedDates()
            {
                CompanyId = companyId
            };
        }

        public OpenClosedDates GetRecurrent(Guid companyId)
        {
            var result = _repositoryOpenClosedDates.GetByKey(NoSqlTables.OpenClosedDates,companyId.ToString());
            if (result != null)
            {
                return result;
            }
            return new OpenClosedDates()
            {
                CompanyId = companyId
            };
        }

        public OpenClosedDates GetAllByYear(Guid companyId, int year)
        {
            var result = (GetByYear(companyId, year) + GetRecurrent(companyId));
            result.SetYear(year);
            return result;
        }

        private string GetKey(Guid companyId, int year)
        {
            return $"{companyId.ToString()}{year}";
        }

        public void AddRecurrentClosedDay(Guid companyId, DateTime recurrentClosed)
        {
            OpenClosedDates closedDates = GetRecurrent(companyId);

            if (closedDates == null)
            {
                closedDates = new OpenClosedDates() { CompanyId = companyId};
            }

            closedDates.ClosedDays.Add(recurrentClosed);
            AddRecurrentClosedDays(closedDates);
        }

        public void AddRecurrentClosedHalfDay(Guid companyId, DateTime recurrentClosed)
        {
            OpenClosedDates closedDates = GetRecurrent(companyId);

            if (closedDates == null)
            {
                closedDates = new OpenClosedDates() { CompanyId = companyId };
            }

            closedDates.ClosedHalfDays.Add(recurrentClosed);
            AddRecurrentClosedDays(closedDates);
        }

        public void AddClosedDay(Guid companyId, DateTime closedDate)
        {
            OpenClosedDates closedDates = GetByYear(companyId, closedDate.Year);

            if (closedDates == null)
            {
                closedDates = new OpenClosedDates() { CompanyId = companyId };
            }

            closedDates.ClosedDays.Add(closedDate);
            AddClosedDays(closedDates, closedDate.Year);
        }

        public void AddClosedHalfDay(Guid companyId, DateTime closedDate)
        {
            OpenClosedDates closedDates = GetByYear(companyId, closedDate.Year);

            if (closedDates == null)
            {
                closedDates = new OpenClosedDates() { CompanyId = companyId };
            }

            closedDates.ClosedHalfDays.Add(closedDate);
            AddClosedDays(closedDates, closedDate.Year);
        }

        public void AddRecurrentClosedDays(OpenClosedDates openClosedDates)
        {
            _repositoryOpenClosedDates.AddOrUpdate(NoSqlTables.OpenClosedDates,openClosedDates.CompanyId.ToString(),openClosedDates);
        }

        public void AddClosedDays(OpenClosedDates openClosedDates, int year)
        {
            string key = GetKey(openClosedDates.CompanyId, year);
            _repositoryOpenClosedDates.AddOrUpdate(NoSqlTables.OpenClosedDates, key, openClosedDates);
        }
    }
}
