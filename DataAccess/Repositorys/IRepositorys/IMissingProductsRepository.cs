using System;
using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IMissingProductsRepository : IRepository<MissingProductValue>
	{
		void Update(MissingProductValue source);
		Task UpdateAsync(MissingProductValue source);

    }
}