using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface ICompanyGlobalRulesService
    {
        GlobalRules GetGlobalRules(Guid companyId);

        GlobalRules UpdateGlobalRules(Guid companyId, GlobalRules updatedGlobalRules);
    }
}