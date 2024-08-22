
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface ITexacoTracactionRepository : IRepository<TexacoTransaction>
	{
		void Update(TexacoTransaction source);
		Task UpdateAsync(TexacoTransaction source);
        IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate);

    }
}