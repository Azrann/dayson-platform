/*using System;
using System.Data;
using System.Data.Entity;

namespace DaysOn.HolidaysBc.SharedKernel.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsInTransaction { get; }

        void SaveChanges();

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void RollBackTransaction();

        void CommitTransaction();

        IDbSet<T> SetEntity<T>() where T : class;
    }
}*/