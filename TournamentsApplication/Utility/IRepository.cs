using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentsApplication.Utilities
{
    interface IRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll(Func<T, object> orderBy, Func<T, bool> searchBy = null);
        public (List<T> items, int TotalPages) Get(int pageNumber, int pageSize, Func<T, object> orderBy, Func<T, bool> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}