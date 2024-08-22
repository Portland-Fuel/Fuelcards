using Portland.Data.Repository.IRepository;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;


namespace Portland.Data.Repository.IRepository
{
	public interface IUkFuelCardRepository : IRepository<UkfuelCard>
	{
		void Update(UkfuelCard source);
		Task UpdateAsync(UkfuelCard source);
        IQueryable<UkfuelCard> Where(Func<UkfuelCard, bool> predicate);
		List<UkfuelCard> RawSQL(string where1, string where2);
    }
}