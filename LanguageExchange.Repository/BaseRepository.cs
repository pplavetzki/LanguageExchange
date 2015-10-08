using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExchange.Interfaces;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace LanguageExchange.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> _objectSet = null;

        public BaseRepository(DbContext context)
        {
            _objectSet = context.Set<T>();
        }

        public IQueryable<T> FindAll()
        {
            return _objectSet;
        }

        public IQueryable<T> FindWhere(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.Where(predicate);
        }

        public void Add(T newEntity)
        {
            _objectSet.Add(newEntity);
        }

        public void Remove(T entity)
        {
            _objectSet.Remove(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.FirstOrDefault(predicate);
        }

        public T FirstOrDefault()
        {
            return _objectSet.FirstOrDefault();
        }

        public DbQuery<T> Include(string path)
        {
            return _objectSet.Include(path);
        }
    }
}
