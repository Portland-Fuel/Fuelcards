using System;
using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using System.Linq;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE22AccountsStoppedRepository : IRepository<KfE22AccountsStopped>
	{
		void Update(KfE22AccountsStopped source);
		Task UpdateAsync(KfE22AccountsStopped source);
		Task<KfE22AccountsStopped> FindAsync(int code);
        KfE22AccountsStopped Find(int CustomerCode);
        IQueryable<KfE22AccountsStopped> Where(Func<KfE22AccountsStopped, bool> predicate);
        KfE22AccountsStopped First(Func<KfE22AccountsStopped, bool> predicate);

    }
}