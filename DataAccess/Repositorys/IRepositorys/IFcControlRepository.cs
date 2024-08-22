using System;
using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcControlRepository : IRepository<FcControl>
	{
		void Update(FcControl source);
		Task UpdateAsync(FcControl source);
        FcControl MostRecentControlId();

    }
}