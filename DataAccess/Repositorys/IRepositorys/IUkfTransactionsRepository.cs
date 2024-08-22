
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IUkfTransactionsRepository : IRepository<UkfTransaction>
	{
		void Update(UkfTransaction source);
		Task UpdateAsync(UkfTransaction source);
        IQueryable<UkfTransaction> Where(Func<UkfTransaction, bool> predicate);
    }
}