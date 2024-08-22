
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcProspectsRepository : IRepository<FcProspect>
	{
		void Update(FcProspect source);
		Task UpdateAsync(FcProspect source);
    }
}