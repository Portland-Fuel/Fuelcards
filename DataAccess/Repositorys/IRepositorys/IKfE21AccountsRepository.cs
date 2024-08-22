using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IKfE21AccountsRepository : IRepository<KfE21Account>
	{
		void Update(KfE21Account source);
		Task UpdateAsync(KfE21Account source);
        IQueryable<KfE21Account> Where(Func<KfE21Account, bool> predicate);
        KfE21Account First(Func<KfE21Account, bool> predicate);
        KfE21Account Find(int CustomerCode);


    }
}