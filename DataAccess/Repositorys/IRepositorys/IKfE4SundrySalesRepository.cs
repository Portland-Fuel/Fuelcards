using System;

using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE4SundrySales : IRepository<KfE4SundrySale>
	{
		void Update(KfE4SundrySale source);
		Task UpdateAsync(KfE4SundrySale source);
		IQueryable<KfE4SundrySale> Where(Func<KfE4SundrySale, bool> predicate);
        KfE4SundrySale First(Func<KfE4SundrySale, bool> predicate);

    }
}