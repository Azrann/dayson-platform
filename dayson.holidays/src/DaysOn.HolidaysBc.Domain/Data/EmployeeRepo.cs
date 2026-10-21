using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Castle.Core;
using Dapper;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Data
{
    [CastleComponent("EmployeeRepo", LifestyleType.Transient)]
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly string _connectionString;

        public EmployeeRepo()
        {
             _connectionString = ConfigurationManager.ConnectionStrings["HolidaysDB"]
                .ConnectionString;
        }

        public Employee GetEmployeeDetails(Guid id)
        {
            Employee result;
            using (IDbConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"SELECT * FROM [Employees] WHERE Id=@id";
                result = sqlConnection.Query<Employee>(sqlQuery,
                    new
                    {
                        id
                    }).FirstOrDefault();
            }

            return result;
        }
    }

    public interface IEmployeeRepo
    {
        Employee GetEmployeeDetails(Guid id);
    }
}
