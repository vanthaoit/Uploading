using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.DataAccess.Interfaces;
using KAS.Uploading.Models.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.BusinessFunctions.Services
{
    public class EFCommonService<TEntity,K>: IEFCommonService<TEntity,K> where TEntity: DomainEntity<K>
    {
        private readonly IEFRepository<TEntity, K> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public EFCommonService(IEFRepository<TEntity, K> repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Task<TEntity> Add(TEntity tEntity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task AddAsync(TEntity tEntity)
        {
            await _repository.AddAsync(tEntity);
            await SaveAsync();

        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                var resp = _repository.FindAll();
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine("error "+e);
                return null;
            }
            
        }

        public IQueryable<TEntity> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(K id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task SaveAsync()
        {
           await _unitOfWork.CommitAsync();
        }

        public void Update(TEntity tEntity)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IEFCommonService<TEntity, K>.AddAsync(TEntity tEntity)
        {
            throw new NotImplementedException();
        }

        Task IEFCommonService<TEntity, K>.Delete(K id)
        {
            throw new NotImplementedException();
        }

        Task IEFCommonService<TEntity, K>.Update(TEntity tEntity)
        {
            throw new NotImplementedException();
        }
    }
}
