using System;
using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;


namespace Portland.Data.Repository.IRepository
{
	public interface IKfE01Repository : IRepository<KfE1E3Transaction>
	{
		void Update(KfE1E3Transaction source);
		Task UpdateAsync(KfE1E3Transaction source);
        IQueryable<KfE1E3Transaction> Where(Func<KfE1E3Transaction, bool> predicate);
    }
}