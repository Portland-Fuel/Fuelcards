
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFixFrequencyRepository : IRepository<FixFrequency>
	{
		void Update(FixFrequency source);
		Task UpdateAsync(FixFrequency source);
        //IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate);

    }
}