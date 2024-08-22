using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using System.Linq;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE11ProductsRepository : IRepository<KfE11Product>
	{
		void Update(KfE11Product source);
		Task UpdateAsync(KfE11Product source);
        IQueryable<KfE11Product> Where(Func<KfE11Product, bool> predicate);
		Task<KfE11Product> FindAsync(int i);
        KfE11Product Find(int ProductCode);

    }
}