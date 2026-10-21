using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("CompanyService", LifestyleType.Transient)]
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _repositoryCompany;
        private readonly INoSqlDb<OpenClosedDates> _repositoryOpenClosedDates;

        public CompanyService(IRepository<Company> repositoryCompany, INoSqlDb<OpenClosedDates> repositoryOpenClosedDates)
        {
            _repositoryCompany = repositoryCompany;
            _repositoryOpenClosedDates = repositoryOpenClosedDates;
        }

        public void AddCompany(Company company)
        {
            _repositoryCompany.Add(company);
            _repositoryCompany.Save();
        }

        public IEnumerable<Company> GetAll()
        {
            return _repositoryCompany.GetAll(x => true);
        }

        public void RemoveCompany(Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            _repositoryCompany.Delete(company);
            _repositoryCompany.Save();
        }

        public Company GetCompanyById(Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            return company;
        }

        public GlobalRules GetGlobalRules(Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            return company.GlobalRules;
        }


    }
}
