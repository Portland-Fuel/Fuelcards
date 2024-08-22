using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcHiddenCardsRepository : IRepository<FcHiddenCard>
	{
		void Update(FcHiddenCard source);
		Task UpdateAsync(FcHiddenCard source);
        IQueryable<FcHiddenCard> Where(Func<FcHiddenCard, bool> predicate);

    }
}