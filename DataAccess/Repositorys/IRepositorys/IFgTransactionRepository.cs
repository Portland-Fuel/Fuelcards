using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IFgTransactionRepository : IRepository<FgTransaction>
	{
		void Update(FgTransaction source);
		Task UpdateAsync(FgTransaction source);

    }
}