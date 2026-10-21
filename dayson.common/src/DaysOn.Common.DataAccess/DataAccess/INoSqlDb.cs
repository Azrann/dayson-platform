namespace DaysOn.Common.DataAccess
{
    public interface INoSqlDb<T> where T : TableEntity
    {
        void CreateTableIfNotExists(string tableName);
        void DeleteTable(string tableName);
        void AddOrUpdate(string tableName, string key, T entity);
        T GetByKey(string tableName, string key);
        void Delete(string tableName, string key);
    }
}
