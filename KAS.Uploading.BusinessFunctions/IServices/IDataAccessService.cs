﻿using KAS.Uploading.Models.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.BusinessFunctions.IServices
{
    public interface IDataAccessService<T>
    {
        Task<T> Get(long key);

        Task<T> Get(int key);

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetDataWithQuery(string query, object parameters);

        Task<IEnumerable<T>> GetDataWithProc(string proc, object parameters);

        Task<IEnumerable<T>> GetDataByFilter(string table = "", params KeyValuePair<string, string>[] filter);

        Task<IEnumerable<T>> GetDataByFilter(params KeyValuePair<string, string>[] filter);

        Task<IEnumerable<T>> GetDataByFilter(params ParameterDefinition[] parameters);

        Task<IEnumerable<T>> GetDataByFilter(object filter, string table = "");

        Task ExecuteProc(string proc, object parameters);

        Task ExecuteStatement(string statement, object parameters);

        Task<long> Insert(T entity);

        Task<long> Insert(IEnumerable<T> entities);

        Task<bool> Delete(T entity);

        Task<bool> Delete(IEnumerable<T> entities);

        Task<bool> Update(T entity);

        Task<bool> Update(IEnumerable<T> entities);
    }
}
