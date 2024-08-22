
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using System.Threading.Tasks;

namespace Portland.Data.Repository.IRepository
{
	public interface IFixedPriceContractsRepository : IRepository<FixedPriceContract>
	{
		void Update(FixedPriceContract source);
		Task UpdateAsync(FixedPriceContract source);
        //IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate);

    }
}