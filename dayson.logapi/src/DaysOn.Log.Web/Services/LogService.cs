using DaysOn.Log.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace DaysOn.Log.Web.Services
{
    public class LogService
    {

        public List<LogItem> GetList()
        {
            var result = new List<LogItem>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["dbConnection"]))
            {
                sqlConnection.Open();

                IEnumerable<LogItem> logs = sqlConnection.Query<LogItem>("Select Top(100) * From dbo.Log where Message Not like 'Version method called%' Order by Id desc");

                foreach (LogItem item in logs)
                {
                    result.Add(item);
                }

                sqlConnection.Close();
            }
            return result;
        }

        public List<LogItem> GetErrors()
        {
            var result = new List<LogItem>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["dbConnection"]))
            {
                sqlConnection.Open();

                IEnumerable<LogItem> logs = sqlConnection.Query<LogItem>("Select Top(100) * From dbo.Log where Level = 'ERROR' Order by Id desc");

                foreach (LogItem item in logs)
                {
                    result.Add(item);
                }

                sqlConnection.Close();
            }
            return result;
        }

        public List<LogItem> CleanLogs()
        {
            var result = new List<LogItem>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["dbConnection"]))
            {
                sqlConnection.Open();

                sqlConnection.Query("Delete From dbo.Log");

                sqlConnection.Close();
            }
            return result;
        }
    }
}
