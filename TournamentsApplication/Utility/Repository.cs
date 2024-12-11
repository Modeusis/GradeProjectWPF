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

        public IEnumerable<T> GetAll(Func<T, object> orderBy = null, Func<T, bool> searchBy = null, Func<T, bool> searchBy2 = null)
        {
            IQueryable<T> query = _dbSet;

            if (searchBy != null)
            {
                query = query.Where(searchBy).AsQueryable();
            }

            if (searchBy2 != null)
            {
                query = query.Where(searchBy2).AsQueryable();
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).AsQueryable();
            }

            return query.ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
