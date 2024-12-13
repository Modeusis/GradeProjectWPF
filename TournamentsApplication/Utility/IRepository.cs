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
        IEnumerable<T> GetAll(Func<T, object> orderBy, Func<T, bool> searchBy = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}