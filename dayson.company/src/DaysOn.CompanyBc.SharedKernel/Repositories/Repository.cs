using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Castle.Core;
using System.Text;

namespace DaysOn.CompanyBc.SharedKernel.Repositories
{
    [ExcludeFromCodeCoverage]
    [CastleComponent(typeof(IRepository<IEntity>), Lifestyle = LifestyleType.Transient)]
    public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private DbContext _session;
        private StringBuilder builder = new StringBuilder();
        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("Argument dbContext cannot be null");
            }

            _session = dbContext;
        }

        public DbContext Context
        {
            get { return _session; }
            set { _session = value; }
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            _session.Database.Log = (x) => builder.AppendLine(x);
            return _session.Set<TEntity>().Where(predicate).ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, List<string> includeElements)
        {
            var myQuery = _session.Set<TEntity>().Where(predicate);
            foreach (var include in includeElements)
            {
                myQuery = myQuery.Include(include);
            }
            return myQuery.ToList();
        }

        public bool Add(TEntity entity)
        {
            if (!IsValid(entity))
            {
                return false;
            }

            try
            {
                _session.Set<TEntity>().Add(entity);
                return _session.Entry(entity).GetValidationResult().IsValid;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message, ex);
                }

                throw new Exception(ex.Message, ex);
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entities = _session.Set<TEntity>().Where(predicate).ToList();
                _session.Set<TEntity>().RemoveRange(entities);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message, ex);
                }

                throw new Exception(ex.Message, ex);
            }
        }

        public bool Delete(TEntity entity)
        {
            if (!IsValid(entity))
            {
                return false;
            }

            try
            {
                _session.Set<TEntity>().Remove(entity);
                return _session.Entry(entity).GetValidationResult().IsValid;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message, ex);
                }

                throw new Exception(ex.Message, ex);
            }
        }

        public bool Update(TEntity entity)
        {
            if (!IsValid(entity))
            {
                return false;
            }

            try
            {
                _session.Set<TEntity>().Attach(entity);
                _session.Entry(entity).State = EntityState.Modified;
                return _session.Entry(entity).GetValidationResult().IsValid;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message, ex);
                }

                throw new Exception(ex.Message, ex);
            }
        }

        public bool IsValid(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity cannot be null");
            }

            return true;
        }

        public void Save()
        {
            _session.SaveChanges();
        }
    }
}