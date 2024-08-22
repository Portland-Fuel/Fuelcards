using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FuelcardBasePricesRepository : Repository<FuelcardBasePrice>, IFuelcardBasePricesRepository
    {
		private readonly FuelcardsContext _db;

		public FuelcardBasePricesRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FuelcardBasePrice source)
		{
			var dbObj = _db.FuelcardBasePrices.FirstOrDefault(s => s.EffectiveFrom == source.EffectiveFrom);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FuelcardBasePrice source)
		{
			var dbObj = _db.FuelcardBasePrices.FirstOrDefault(s => s.EffectiveFrom == source.EffectiveFrom);
			if (dbObj is null) await _db.FuelcardBasePrices.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(FuelcardBasePrice dbObj, FuelcardBasePrice source)
		{
            dbObj.EffectiveFrom = dbObj.EffectiveFrom;
            dbObj.EffectiveTo = source.EffectiveTo;
            dbObj.BasePrice = source.BasePrice;
        }
    }
}