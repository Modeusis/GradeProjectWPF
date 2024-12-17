using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utilities;

namespace TournamentsApplication.Utility
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> GetAll(Func<T, object> orderBy = null, Func<T, bool> searchBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (searchBy != null)
            {
                query = query.Where(searchBy).AsQueryable();
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).AsQueryable();
            }

            return query;
        }
        public (List<T> items, int TotalPages) Get(int pageNumber, int pageSize, Func<T, object> orderBy, Func<T, bool> filter = null)
        {
            var totalCount = _dbSet.Where(filter).Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var items = _dbSet.Where(filter)
                .OrderBy(orderBy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalPages);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            var keyProperty = _context.Model.FindEntityType(typeof(T))
                .FindPrimaryKey().Properties.FirstOrDefault();
            if (keyProperty != null)
            {
                var keyValue = keyProperty.PropertyInfo.GetValue(entity);
                var trackedEntity = _dbSet.Find(keyValue);

                if (trackedEntity != null)
                {
                    _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    _dbSet.Update(entity);
                }
            }
        }

        public void Delete(T entity)
        {
            var keyProperty = _context.Model.FindEntityType(typeof(T))
                .FindPrimaryKey().Properties.FirstOrDefault();

            if (keyProperty != null)
            {
                var keyValue = keyProperty.PropertyInfo.GetValue(entity);
                var trackedEntity = _dbSet.Find(keyValue);

                if (trackedEntity != null)
                {
                    // Если сущность отслеживается, удаляем её
                    _dbSet.Remove(trackedEntity);
                }
                else
                {
                    // Если не отслеживается, прикрепляем и удаляем
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }
    }
}
