
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface ITransactionalSiteSurchargeRepository : IRepository<TransactionSiteSurcharge>
	{
		void Update(TransactionSiteSurcharge source);
		Task UpdateAsync(TransactionSiteSurcharge source);

    }
}