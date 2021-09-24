using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.BusinessFunctions.IServices
{
    public interface IDapperDataService<TEntity> : IDataAccessService<TEntity> where TEntity : class
    {
    }
}
