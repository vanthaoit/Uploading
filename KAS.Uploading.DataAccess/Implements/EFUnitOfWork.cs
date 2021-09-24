using KAS.Uploading.DataAccess.Interfaces;
using System.Threading.Tasks;

namespace KAS.Uploading.DataAccess.Implements
{
    public class EFUnitOfWork:IUnitOfWork
    {
		private readonly ApplicationDbContext _context;

		public EFUnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		public void Commit()
		{
			_context.SaveChanges();
		}

		public async Task CommitAsync()
		{
			await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}