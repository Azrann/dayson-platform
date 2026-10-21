using System;
using System.Collections.Generic;
using DaysOn.CompanyBc.BoundedContext.Entities;

namespace DaysOn.CompanyBc.BoundedContext.Services
{
    public interface ICompanyService
    {
        void AddCompany(Company company);
        void RemoveCompany(Guid companyId);
        IEnumerable<Company> GetAll();
        void AddTeam(Team team, Guid companyId);
        void UpdateTeam(Team team, Guid companyId);
        void RemoveTeam(Guid teamId, Guid companyId);
        IEnumerable<Team> GetTeams(Guid companyId);
        Team GetTeam(Guid teamId, Guid companyId);
        void AddEmployee(Employee employee, Guid companyId);
        void RemoveEmployee(Employee employee, Guid companyId);
        void UpdateEmployee(Employee employee, Guid companyId);
        Employee GetEmployee(Guid employeeId, Guid companyId);
    }
}
