using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IKfE5Repository : IRepository<KfE5Stock>
	{
		void Update(KfE5Stock source);
		Task UpdateAsync(KfE5Stock source);
        IQueryable<KfE5Stock> Where(Func<KfE5Stock, bool> predicate);
        KfE5Stock First(Func<KfE5Stock, bool> predicate);

    }
}