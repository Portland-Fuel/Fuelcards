using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcMaskedCardsRepository : IRepository<FcMaskedCard>
	{
		void Update(FcMaskedCard source);
		Task UpdateAsync(FcMaskedCard source);
        IQueryable<FcMaskedCard> Where(Func<FcMaskedCard, bool> predicate);

    }
}