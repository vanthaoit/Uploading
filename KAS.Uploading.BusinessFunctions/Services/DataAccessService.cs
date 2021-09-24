using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.DataAccess.Repositories;
using KAS.Uploading.Models.Structs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.BusinessFunctions.Services
{
    internal class DataAccessService<TEntity> : IDataAccessService<TEntity> where TEntity : class
    {
        private readonly string _conn;

        public DataAccessService(string connectionStr)
        {
            _conn = connectionStr;
        }


        public async Task<bool> Delete(TEntity entity)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Delete(entity);
            }
        }

        public async Task<bool> Delete(IEnumerable<TEntity> entities)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Delete(entities);
            }
        }

        public async Task ExecuteProc(string proc, object parameters)
        {
            using (var conn = new SqlConnection(_conn))
            {
                await new Repository<TEntity>(conn).ExecuteProc(proc, parameters);
            }
        }

        public async Task ExecuteStatement(string statement, object parameters)
        {
            using (var conn = new SqlConnection(_conn))
            {
                await new Repository<TEntity>(conn).ExecuteStatement(statement, parameters);
            }
        }

        public async Task<TEntity> Get(long key)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Get(key);
            }
        }

        public async Task<TEntity> Get(int key)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Get(key);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetAll();
            }
        }

        public async Task<IEnumerable<TEntity>> GetDataByFilter(string table = "", params KeyValuePair<string, string>[] filter)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetDataByFilter(table, filter);
            }
        }

        public async Task<IEnumerable<TEntity>> GetDataByFilter(params KeyValuePair<string, string>[] filter)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetDataByFilter(filter);
            }
        }

        public async Task<IEnumerable<TEntity>> GetDataByFilter(params ParameterDefinition[] parameters)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetDataByFilter(parameters);
            }
        }

        public async Task<IEnumerable<TEntity>> GetDataByFilter(object filter, string table = "")
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetDataByFilter(filter);
            }
        }

        public async Task<IEnumerable<TEntity>> GetDataWithProc(string proc, object parameters)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetDataWithProc(proc, parameters);
            }
        }

        public async Task<IEnumerable<TEntity>> GetDataWithQuery(string query, object parameters)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).GetDataWithQuery(query, parameters);
            }
        }

        public async Task<long> Insert(TEntity entity)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Insert(entity);
            }
        }

        public async Task<long> Insert(IEnumerable<TEntity> entities)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Insert(entities);
            }
        }

        public async Task<bool> Update(TEntity entity)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Update(entity);
            }
        }

        public async Task<bool> Update(IEnumerable<TEntity> entities)
        {
            using (var conn = new SqlConnection(_conn))
            {
                return await new Repository<TEntity>(conn).Update(entities);
            }
        }
    }
}
