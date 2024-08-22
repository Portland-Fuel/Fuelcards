using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE23NewClosedSitesRepository : IRepository<KfE23NewClosedSite>
	{
		void Update(KfE23NewClosedSite source);
		Task UpdateAsync(KfE23NewClosedSite source);
		Task<KfE23NewClosedSite> FindAsync(int code);
        KfE23NewClosedSite Find(int CustomerCode);
        IQueryable<KfE23NewClosedSite> Where(Func<KfE23NewClosedSite, bool> predicate);
        KfE23NewClosedSite First(Func<KfE23NewClosedSite, bool> predicate);

    }
}