using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Castle.Core;
using Dapper;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Data
{
    [CastleComponent("HolidayRepo", LifestyleType.Transient)]
    public class HolidayRepo : IHolidayRepo
    {
        private readonly string _connectionString;

        public HolidayRepo()
        {
             _connectionString = ConfigurationManager.ConnectionStrings["HolidaysDB"]
                .ConnectionString;
        }

        public HolidayRequestDto GetHoliday(Guid id)
        {
            HolidayRequestDto result;
            using (IDbConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"SELECT * FROM [HolidayRequests] WHERE Id=@id";
                result = sqlConnection.Query<HolidayRequestDto>(sqlQuery,
                    new
                    {
                        id
                    }).FirstOrDefault();
            }

            return result;
        }


        public void ChangeStatus(Guid id,Status status)
        {
            using (IDbConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE[HolidayRequests]
                                    SET Status = @status
                                    WHERE Id = @id";
                sqlConnection.Execute(sqlQuery,
                    new
                    {
                        Id = id,
                        Status = status
                    });
            }
        }

        public void AddHoliday(Guid id, DateTime from, DateTime to, Status status, decimal numberOfDays, string notes, Guid employeeId, int year)
        {
            using (IDbConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"INSERT INTO[dbo].[HolidayRequests]
                                    ([Id],[From],[To],[Status],[NumberOfDays],[Notes],[EmployeeId],[Year],[CreatedOn],[LastUpdated])
                                    VALUES
                                    (@Id,@From,@To,@Status,@NumberOfDays,@Notes,@EmployeeId,@Year, getDate(), getDate())";
                sqlConnection.Execute(sqlQuery,
                    new
                    {
                        Id = id,
                        From= from,
                        To= to,
                        Status = status,
                        NumberOfDays = numberOfDays,
                        Notes = notes,
                        EmployeeId = employeeId,
                        Year = year
                    });
            }
        }
    }
}
