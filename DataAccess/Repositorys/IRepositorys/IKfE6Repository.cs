using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE6Repository : IRepository<KfE6Transfer>
	{
		void Update(KfE6Transfer source);
		Task UpdateAsync(KfE6Transfer source);
        IQueryable<KfE6Transfer> Where(Func<KfE6Transfer, bool> predicate);
        KfE6Transfer First(Func<KfE6Transfer, bool> predicate);

    }
}