
using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE2DeliveryRepository : IRepository<KfE2Delivery>
	{
		void Update(KfE2Delivery source);
		Task UpdateAsync(KfE2Delivery source);
        IQueryable<KfE2Delivery> Where(Func<KfE2Delivery, bool> predicate);

    }
}