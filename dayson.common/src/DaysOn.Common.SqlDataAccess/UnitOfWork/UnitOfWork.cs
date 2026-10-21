/*using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using DaysOn.HolidaysBc.SharedKernel.CodeContract;

namespace DaysOn.HolidaysBc.SharedKernel.UnitOfWork
{
    [ExcludeFromCodeCoverage]
    [CastleComponent(typeof(IUnitOfWork), Lifestyle = LifestyleType.Transient)]
    public class UnitOfWork : DbContext, IUnitOfWork
    {
            protected DbTransaction MDatabaseTransaction;
            protected bool MDisposed;

            #region Properties
            public bool IsInTransaction
            {
                get
                {
                    if (MDatabaseTransaction != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            #endregion

            #region Public Methods
            public virtual void BeginTransaction()
            {
                BeginTransaction(IsolationLevel.ReadCommitted);
            }

            public virtual void BeginTransaction(IsolationLevel isolationLevel)
            {
                if (MDatabaseTransaction != null)
                {
                    throw new ApplicationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                    "Please commit or rollback the existing transaction before starting a new one.");
                }
                OpenConnection();
                MDatabaseTransaction = ((IObjectContextAdapter)this).ObjectContext.Connection.BeginTransaction(isolationLevel);
            }

            public virtual void RollBackTransaction()
            {
                if (MDatabaseTransaction == null)
                {
                    throw new ApplicationException("Cannot rollback a transaction if one does not exist.");
                }

                MDatabaseTransaction.Rollback();
            }

            public virtual void CommitTransaction()
            {
                if (MDatabaseTransaction == null)
                {
                    throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
                }

                try
                {
                    ((IObjectContextAdapter)this).ObjectContext.SaveChanges();
                    MDatabaseTransaction.Commit();
                }
                catch
                {
                    MDatabaseTransaction.Rollback();
                    throw;
                }
                finally
                {
                    ReleaseCurrentTransaction();
                }
            }

        public virtual IDbSet<T> SetEntity<T>() where T : class
        {
            return Set<T>();
        }

        public virtual void SaveChanges()
            {
                InternalSaveChanges();

            }

            public virtual void SaveChanges(SaveOptions saveOptions)
            {
                if (IsInTransaction)
                {
                    throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");
                }

                InternalSaveChanges();

            }

            #region Implementation of IDisposable

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public virtual void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Disposes off the managed and unmanaged resources used.
            /// </summary>
            /// <param name="disposing"></param>
            private void Dispose(bool disposing)
            {
                if (!disposing)
                {
                    return;
                }

                if (MDisposed)
                {
                    return;
                }

                MDisposed = true;
            }


            #endregion

            #endregion

            #region Private Methods
            private void OpenConnection()
            {
                var objectContext = ((IObjectContextAdapter)this).ObjectContext;

                if (objectContext.Connection.State != ConnectionState.Open)
                {
                    objectContext.Connection.Open();
                }
            }

            /// <summary>
            /// Releases the current transaction.
            /// </summary>
            private void ReleaseCurrentTransaction()
            {
                if (MDatabaseTransaction != null)
                {
                    MDatabaseTransaction.Dispose();
                    MDatabaseTransaction = null;
                }
            }

            private void InternalSaveChanges()
            {
                try
                {
                    var context = ((IObjectContextAdapter)this).ObjectContext;

                    context.SavingChanges += ObjectContextSavingChanges;
                    context.SaveChanges();
                }
                catch (UpdateException e)
                {
                    var sqlException = e.InnerException as SqlException;

                    if (sqlException != null && sqlException.Number == 2627)
                    {
                        throw new Exception("Unique key is violated", e);
                    }

                    throw;
                }
            }

            private static void ObjectContextSavingChanges(object sender, EventArgs e)
            {
                var context = sender as ObjectContext;
                Contract.Requires<ArgumentNullException>(context == null, "ObjectContext");
                ValidateEntities(context);
            }

            private static void ValidateEntities(ObjectContext context)
            {
                
            }

            #endregion

        }
}
*/