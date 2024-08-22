using DataAccess.Repository;
using DataAccess.Fuelcards;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class CustomerMultipleAccountSameNetworkRepository : Repository<CustomerMultipleAccountSameNetwork>, ICustomerMultipleAccountSameNetworkRepository
    {
		private readonly FuelcardsContext _db;

		public CustomerMultipleAccountSameNetworkRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(CustomerMultipleAccountSameNetwork source)
		{
			var dbObj = _db.CustomerMultipleAccountSameNetworks.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(CustomerMultipleAccountSameNetwork source)
		{
			var dbObj = _db.CustomerMultipleAccountSameNetworks.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.CustomerMultipleAccountSameNetworks.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        
        private void UpdateDbObject(CustomerMultipleAccountSameNetwork dbObj, CustomerMultipleAccountSameNetwork source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.PortlandId = source.PortlandId;
            dbObj.FcAccount = source.FcAccount;
			dbObj.Network = source.Network;
            
        }
    }
}