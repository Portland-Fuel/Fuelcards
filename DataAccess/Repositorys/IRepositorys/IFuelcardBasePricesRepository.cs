
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IFuelcardBasePricesRepository : IRepository<FuelcardBasePrice>
	{
		void Update(FuelcardBasePrice source);
		Task UpdateAsync(FuelcardBasePrice source);
        //IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate);

    }
}