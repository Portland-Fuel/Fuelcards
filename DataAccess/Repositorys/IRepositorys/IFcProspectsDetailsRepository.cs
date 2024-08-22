using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcProspectsDetailsRepository : IRepository<FcProspectsDetail>
	{
		void Update(FcProspectsDetail source);
		Task UpdateAsync(FcProspectsDetail source);
    }
}