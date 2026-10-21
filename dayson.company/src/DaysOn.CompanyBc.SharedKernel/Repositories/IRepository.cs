using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DaysOn.CompanyBc.SharedKernel.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        DbContext Context { get; set; }

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, List<string> includeElements);

        bool Add(TEntity entity);

        bool Delete(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> predicate);

        bool Update(TEntity entity);

        bool IsValid(TEntity entity);

        void Save();
    }
}