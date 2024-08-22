
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface ITransactionalSitesRepository : IRepository<TransactionalSite>
	{
		void Update(TransactionalSite source);
		Task UpdateAsync(TransactionalSite source);
        //IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate);

    }
}