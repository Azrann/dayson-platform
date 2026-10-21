using System;
using System.Collections.Generic;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface ICompanyService
    {
        void AddCompany(Company company);
        IEnumerable<Company> GetAll();
        void RemoveCompany(Guid companyId);
        GlobalRules GetGlobalRules(Guid companyId);
        Company GetCompanyById(Guid companyId);
    }
}
