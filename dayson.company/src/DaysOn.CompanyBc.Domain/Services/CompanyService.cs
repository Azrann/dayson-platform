using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Castle.Core;
using Dapper;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.CompanyBc.BoundedContext.Events;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.CompanyBc.BoundedContext.Services
{
    [CastleComponent("CompanyService", LifestyleType.Transient)]
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _repositoryCompany;
        private readonly ITeamService _teamService;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly IRepository<Team> _repositoryTeam;

        public CompanyService(IRepository<Company> repositoryCompany, ITeamService teamService, IRepository<Employee> repositoryEmployee, IRepository<Team> repositoryTeam)
        {
            _repositoryCompany = repositoryCompany;
            _teamService = teamService;
            _repositoryEmployee = repositoryEmployee;
            _repositoryTeam = repositoryTeam;
        }

        public void AddCompany(Company company)
        {
            _repositoryCompany.Add(company);
            _repositoryCompany.Save();
        }

        public IEnumerable<Company> GetAll()
        {
            var includeList = new List<string> { "Teams", "Employees" };
            return _repositoryCompany.GetAll(x => true , includeList);
        }

        public void AddTeam(Team team, Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }
            company.AddTeam(team);
            _repositoryCompany.Save();

        }

        public void UpdateTeam(Team team, Guid companyId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["CompanyDB"].ConnectionString))
            {
                string updateQuery = @"UPDATE [dbo].[Teams] SET Name = @Name WHERE Id = @Id AND Company_Id = @CompanyId";

                var result = db.Execute(updateQuery, new
                {
                    team.Name,
                    team.Id,
                    CompanyId = companyId
                });
            }
        }

        public void RemoveTeam(Guid teamId, Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId, new List<string> { "Teams" }).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }
           
            _teamService.RemoveAllTeamMembers(teamId);
            _teamService.RemoveTemporalManager(teamId);
            _teamService.RemoveManager(teamId);

            _teamService.RemoveTeam(teamId);
        }

        public IEnumerable<Team> GetTeams(Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId, new List<string> { "Teams" , "Teams.Manager", "Teams.TemporalManager" }).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            return company.Teams;
        }

        public Team GetTeam(Guid teamId, Guid companyId)
        {
            var includeList = new List<string> {"Teams"};
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId, includeList).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            if (company.Teams.FirstOrDefault(x => x.Id == teamId) != null)
            {
                includeList = new List<string> { "TeamMembers" , "Manager", "TemporalManager"};
                return _repositoryTeam.GetAll(x => x.Id == teamId, includeList).FirstOrDefault();
            }

            return null;
        }

        public void RemoveCompany(Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }
            List<Guid> teamsId = company.Teams.Select(x => new Guid(x.Id.ToString())).ToList();
            foreach (var teamId in teamsId)
            {
                RemoveTeam(teamId, companyId);
            }

            List<Guid> employeeIds = company.Employees.Select(x => new Guid(x.Id.ToString())).ToList();
            foreach (var employeeId in employeeIds)
            {
                var emp = company.Employees.First(x => x.Id == employeeId);
                RemoveEmployee(emp, companyId);
            }

            _repositoryCompany.Delete(company);
            _repositoryCompany.Save();
        }

        public void AddEmployee(Employee employee, Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            company.AddEmployee(employee);
            _repositoryCompany.Save();

            DomainEvents.Raise(new EmployeeCreatedEvent
            (employee.Id, employee.Firstname, employee.Lastname,employee.EmailAddress,companyId,Guid.NewGuid(), employee.Role));
        }

        public void UpdateEmployee(Employee employee, Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId, new List<string> { "Employees" }).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            company.UpdateEmployee(employee);
            _repositoryCompany.Save();

            DomainEvents.Raise(new EmployeeUpdatedEvent
                (employee.Id, employee.EmailAddress, companyId, Guid.NewGuid(), employee.Role));
        }

        public void RemoveEmployee(Employee employee, Guid companyId)
        {
            Company company = _repositoryCompany.GetAll(x => x.Id == companyId).FirstOrDefault();

            if (company == null)
            {
                throw new ArgumentException(string.Format(Error.CompanyNotFound, companyId));
            }

            if (company.Employees.Any(x => x.Id == employee.Id))
            {
                _repositoryEmployee.Delete(employee);
                _repositoryEmployee.Save();
            }
        }

        public Employee GetEmployee(Guid employeeId, Guid companyId)
        {
            var employee = _repositoryEmployee.GetAll(emp => emp.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            if (employee.CompanyId == companyId)
            {
                return employee;
            }

            return null; 
        }
    }
}
