using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE21AccountsRepository : Repository<KfE21Account>, IKfE21AccountsRepository
    {
		private readonly FuelcardsContext _db;

		public KfE21AccountsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE21Account source)
		{
			var dbObj = _db.KfE21Accounts.FirstOrDefault(s => s.CustomerAccountCode == source.CustomerAccountCode);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public KfE21Account Find(int code)
        {
           return _db.KfE21Accounts.Find(code);
        }

        public async Task UpdateAsync(KfE21Account source)
		{
			var dbObj = _db.KfE21Accounts.FirstOrDefault(e=>e.CustomerAccountCode == source.CustomerAccountCode);
			if (dbObj is null) await _db.KfE21Accounts.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE21Account> Where(Func<KfE21Account, bool> predicate)
        {
            return _db.KfE21Accounts.Where(predicate).AsQueryable();
        }
        public KfE21Account First(Func<KfE21Account, bool> predicate)
        {
            return _db.KfE21Accounts.Where(predicate).First();
        }
        private void UpdateDbObject(KfE21Account dbObj, KfE21Account source)
		{
            dbObj.CustomerAccountCode = source.CustomerAccountCode;
            dbObj.PortlandId = source.PortlandId;
            dbObj.CustomerAccountSuffix = source.CustomerAccountSuffix;
            dbObj.Date = source.Date;
            dbObj.Time = source.Time;
            dbObj.ActionStatus = source.ActionStatus;
            dbObj.Name = source.Name;
            dbObj.AddressLine1 = source.AddressLine1;
            dbObj.AddressLine2 = source.AddressLine2;
            dbObj.Town = source.Town;
            dbObj.County = source.County;
            dbObj.Postcode = source.Postcode;

        }
    }
}