using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.BusinessFunctions.Implement
{
    public class ProductService : IEFCommonService<Product, int>
    {
        public ProductService()
        {

        }
        public Task<Product> Add(Product tEntity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> AddAsync(Product tEntity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task Update(Product tEntity)
        {
            throw new NotImplementedException();
        }
    }
}
