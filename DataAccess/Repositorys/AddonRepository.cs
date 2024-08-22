using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{
	
	public class AddonRepository : Repository<CustomerPricingAddon>, IAddonRepository
	{
		private readonly FuelcardsContext _db;

		public AddonRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(CustomerPricingAddon source)
		{
			var dbObj = _db.CustomerPricingAddons.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

		public async Task UpdateAsync(CustomerPricingAddon source)
		{
			var dbObj = _db.CustomerPricingAddons.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.CustomerPricingAddons.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}

		private void UpdateDbObject(CustomerPricingAddon dbObj, CustomerPricingAddon source)
		{
			dbObj.Id = source.Id;
			dbObj.EffectiveDate = source.EffectiveDate;
			dbObj.PortlandId = source.PortlandId;
			dbObj.Network = source.Network;
			dbObj.Addon = source.Addon;

        }
	}
}