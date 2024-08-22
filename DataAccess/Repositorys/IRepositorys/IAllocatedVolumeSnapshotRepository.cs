using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IAllocatedVolumeSnapshotRepository : IRepository<AllocatedVolumeTemp>
	{
		void Update(AllocatedVolumeTemp source);
		void Delete(AllocatedVolumeTemp source);
        Task UpdateAsync(AllocatedVolumeTemp source);
		void DeleteAll();
    }
}