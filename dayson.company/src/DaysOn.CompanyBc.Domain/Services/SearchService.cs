using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Castle.Core;
using Dapper;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.Models.Dtos;

namespace DaysOn.CompanyBc.BoundedContext.Services
{
    [CastleComponent("SearchService", LifestyleType.Transient)]
    public class SearchService : ISearchService
    {
        private readonly string _connectionString;

        public SearchService()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CompanyDB"]
                .ConnectionString;
        }

        public IEnumerable<EmployeeDto> Search(List<Guid> teamIds, string searchString, Guid companyId)
        {
            List<EmployeeDto> employeesResult = new List<EmployeeDto>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = conn.Query(BuildSqlQuery(teamIds,searchString,companyId),
                new
                {
                    SearchTerm = searchString ?? string.Empty,
                    TeamIds = teamIds?.Select(x => x.ToString()) ?? new List<string>(),
                    CompanyId = companyId
                });

                foreach (var employee in result)
                {
                    EmployeeDto emp = new EmployeeDto()
                    {
                        Id = employee.Id,
                        Firstname = employee.Firstname,
                        DateOfBirth = employee.DateOfBirth,
                        EmailAddress = employee.EmailAddress,
                        JobTitle = employee.JobTitle,
                        Lastname = employee.Lastname,
                        ManagerOf = null,
                        Middlename = employee.Middlename,
                         OthersJson = employee.OthersJson,
                         PhoneNumber = employee.PhoneNumber,
                         PictureUrl = employee.PictureUrl,
                         Role = employee.Role,
                         TemporalManagerOf = null,
                         Company = new IdNameDto() { Id = employee.CompanyId},
                        MyTeam = new IdNameDto()
                        {
                            Id = employee.MyTeamId,
                            Name = employee.TeamName
                        }

                    };
                    employeesResult.Add(emp);
                }

                return employeesResult;
            }
        }

        private static string BuildSqlQuery(List<Guid> teamIds, string searchString, Guid companyId)
        {
            var result = @"SELECT E.* , T.Name as TeamName 
                                            FROM Teams as T
                                            INNER JOIN (
                                            SELECT * FROM Employees
                                                WHERE CompanyId = @CompanyId";

            if (teamIds != null && teamIds.Any())
            {
                result += " AND MyTeamId in @TeamIds";
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                result += @" AND (FirstName Like  '%' + @SearchTerm + '%'
                                                OR Middlename like '%' + @SearchTerm + '%'
                                                OR Lastname like '%' + @SearchTerm + '%'
                                                OR DateOfBirth like '%' + @SearchTerm + '%'
                                                OR PhoneNumber like '%' + @SearchTerm + '%'
                                                OR EmailAddress like '%' + @SearchTerm + '%'
                                                OR JobTitle like '%' + @SearchTerm + '%'
                                                OR ""Role"" like '%' + @SearchTerm + '%'
                                                OR OthersJson like '%' + @SearchTerm + '%'
                                                OR @SearchTerm = '')";
            }

            result +=     @")  as E
                                                ON(T.Id = E.MyTeamId)";
            return result;
        }
    }

    public interface ISearchService
    {
        IEnumerable<EmployeeDto> Search(List<Guid> teamIds, string searchString, Guid companyId);
    }
}
