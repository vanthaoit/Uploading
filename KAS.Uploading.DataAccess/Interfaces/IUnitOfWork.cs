using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KAS.Uploading.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}
