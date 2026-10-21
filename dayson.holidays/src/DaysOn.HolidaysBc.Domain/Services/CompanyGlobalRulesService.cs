using System;
using System.Linq;
using System.Reflection;
using Castle.Core;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;
using log4net;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("CompanyGlobalRulesService", LifestyleType.Transient)]
    public class CompanyGlobalRulesService : ICompanyGlobalRulesService
    {
        private readonly IRepository<GlobalRules> _repositoryGlobalRules;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyGlobalRulesService(IRepository<GlobalRules> repositoryGlobalRules)
        {
            _repositoryGlobalRules = repositoryGlobalRules;
        }

        public GlobalRules GetGlobalRules(Guid companyId)
        {
            var globalRules = _repositoryGlobalRules.GetAll(x => x.CompanyId == companyId).FirstOrDefault();

            if (globalRules == null)
            {
                _logger.Warn(string.Format(Error.GlobalRulesNotFound, companyId));
                return new GlobalRules(Guid.NewGuid(), companyId);
            }

            return globalRules;
        }

        public GlobalRules UpdateGlobalRules(Guid companyId, GlobalRules updatedGlobalRules)
        {
            if (updatedGlobalRules == null)
            {
                throw new ArgumentException(Error.GlobalRulesIsNull);
            }

            var globalRules = _repositoryGlobalRules.GetAll(x => x.CompanyId == companyId).FirstOrDefault();

            if (globalRules == null)
            {
                updatedGlobalRules.CompanyId = companyId;
                _repositoryGlobalRules.Add(updatedGlobalRules);
                return updatedGlobalRules;
            }

            globalRules.DefaultNumberOfHolidays = updatedGlobalRules.DefaultNumberOfHolidays;
            globalRules.MaxConsecutiveDaysOff = updatedGlobalRules.MaxConsecutiveDaysOff;
            globalRules.MaxDaysCarriedOverNextYear = updatedGlobalRules.MaxDaysCarriedOverNextYear;
            globalRules.MinDaysAdvanceNoticeHolidays = updatedGlobalRules.MinDaysAdvanceNoticeHolidays;
            globalRules.InitializeWorkinDaysOfTheWeek(updatedGlobalRules.WorkingDaysOfTheWeek);
            globalRules.YearStartDay = updatedGlobalRules.YearStartDay;
            globalRules.CountryId = updatedGlobalRules.CountryId;
            globalRules.OpenDuringBankHolidays = updatedGlobalRules.OpenDuringBankHolidays;
            _repositoryGlobalRules.Save();

            return globalRules;
        }
    }
}
