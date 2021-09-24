using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.BusinessFunctions.IServices
{
    public interface IEFCommonService<TEntity,K> where TEntity : class
    {
        Task<TEntity> Add(TEntity tEntity);

        Task<TEntity> AddAsync(TEntity tEntity);

        Task Update(TEntity tEntity);

        Task Delete(K id);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(string keyword);

        TEntity GetById(K id);

        void Save();

        Task SaveAsync();
    }
}
