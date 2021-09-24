using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.DataAccess.Interfaces
{
    public interface IEFRepository<T, K> where T : class
    {
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        Task<IQueryable<T>> FindAllAsync(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(K id);

        void RemoveMultiple(List<T> entities);
    }
}
