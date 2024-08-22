using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IAllocatedVolumeRepository : IRepository<AllocatedVolume>
	{
		void Update(AllocatedVolume source);
		void Delete(AllocatedVolume source);
        Task UpdateAsync(AllocatedVolume source);
		void DeleteAll();
    }
}