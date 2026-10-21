using Dapper;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DaysOn.Common.DataAccess
{
    public class DapperNoSql<T> : INoSqlDb<T> where T : TableEntity
    {
        private string _connectionString;
        private const string createTable = "if not exists (select * from sysobjects where name='{0}' and xtype='U') create table {0}(Id varchar(64) not null, Value VARCHAR(MAX)) ";
        private const string insertOrUpdate = "if not exists (select * from {0} where Id='{1}') " +
                                              " begin" +
                                              "	Insert Into {0} VALUES ('{1}','{2}');" +
                                              " end " +
                                              " else " +
                                              " begin " +
                                              "	Update {0} set Value = '{2}' Where  Id='{1}'" +
                                              " end";
        private const string getByKey = "Select * from {0} where Id='{1}'";
        private const string deleteByKey = "Delete from {0} where Id='{1}'";
        private const string deleteTable = "DROP TABLE {0}";

        public DapperNoSql()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NoSQL"].ConnectionString;
        }

        public void AddOrUpdate(string tableName, string key, T entity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(string.Format(insertOrUpdate, tableName, key, JsonConvert.SerializeObject(entity)));
            }
        }

        public void CreateTableIfNotExists(string tableName)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(string.Format(createTable,tableName));
            }
        }

        public void Delete(string tableName, string key)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(string.Format(deleteByKey, tableName, key));
            }
        }

        public void DeleteTable(string tableName)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(string.Format(deleteTable, tableName));
            }
        }

        public T GetByKey(string tableName, string key)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var row = db.Query<TableRow>(string.Format(getByKey, tableName,key)).FirstOrDefault();
                if (row == null || string.IsNullOrEmpty(row.Value))
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<T>(row.Value);
            }
        }
    }
}
