using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace Portland.Data.Repository.IRepository
{
	public interface IAddonRepository : IRepository<CustomerPricingAddon>
	{
		void Update(CustomerPricingAddon source);
		Task UpdateAsync(CustomerPricingAddon source);
	}
}