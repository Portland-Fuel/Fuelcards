using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcRequiredEdiReportsRepository : IRepository<FcRequiredEdiReport>
	{
		void Update(FcRequiredEdiReport source);
		Task UpdateAsync(FcRequiredEdiReport source);
        IQueryable<FcRequiredEdiReport> Where(Func<FcRequiredEdiReport, bool> predicate);

    }
}