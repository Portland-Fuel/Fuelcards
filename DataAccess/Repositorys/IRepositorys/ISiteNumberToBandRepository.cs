
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface ISiteNumberToBandRepository : IRepository<SiteNumberToBand>
	{
		void Update(SiteNumberToBand source);
		Task UpdateAsync(SiteNumberToBand source);
		Task<SiteNumberToBand> GetSpecificSurcharge(DateOnly date, int Site, int network);
        //IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate);

    }
}