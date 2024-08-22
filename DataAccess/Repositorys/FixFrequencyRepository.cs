using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FixFrequencyRepository : Repository<FixFrequency>, IFixFrequencyRepository
    {
		private readonly FuelcardsContext _db;

		public FixFrequencyRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FixFrequency source)
		{
			var dbObj = _db.FixFrequencies.FirstOrDefault(s => s.FrequencyId == source.FrequencyId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FixFrequency source)
		{
			var dbObj = _db.FixFrequencies.FirstOrDefault(s => s.FrequencyId == source.FrequencyId);
			if (dbObj is null) await _db.FixFrequencies.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(FixFrequency dbObj, FixFrequency source)
		{
            dbObj.FrequencyId = dbObj.FrequencyId;
            dbObj.FrequencyPeriod = source.FrequencyPeriod;
            dbObj.NoDays = source.NoDays;
            dbObj.PeriodsPerYear = source.PeriodsPerYear;
        }
    }
}