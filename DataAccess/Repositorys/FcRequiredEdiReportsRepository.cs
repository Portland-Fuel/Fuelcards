using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FcRequiredEdiReportsRepository : Repository<FcRequiredEdiReport>, IFcRequiredEdiReportsRepository
    {
		private readonly FuelcardsContext _db;

		public FcRequiredEdiReportsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcRequiredEdiReport source)
		{
			var dbObj = _db.FcRequiredEdiReports.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        

        public async Task UpdateAsync(FcRequiredEdiReport source)
		{
			var dbObj = _db.FcRequiredEdiReports.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.FcRequiredEdiReports.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<FcRequiredEdiReport> Where(Func<FcRequiredEdiReport, bool> predicate)
        {
            return _db.FcRequiredEdiReports.Where(predicate).AsQueryable();
        }
        private void UpdateDbObject(FcRequiredEdiReport dbObj, FcRequiredEdiReport source)
		{
            dbObj.IntroducerId = dbObj.IntroducerId;
            dbObj.CustomerId = dbObj.CustomerId;
            dbObj.Keyfuels = dbObj.Keyfuels;
            dbObj.UkFuels = dbObj.UkFuels;
            dbObj.Texaco = dbObj.Texaco;
            dbObj.Fuelgenie = dbObj.Fuelgenie;
        }
    }
}