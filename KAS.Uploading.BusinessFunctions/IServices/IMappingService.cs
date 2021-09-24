using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.BusinessFunctions.IServices
{
    public interface IMappingService<TFrom, MapTo> : IDataAccessService<TFrom>
    {
    }
}
