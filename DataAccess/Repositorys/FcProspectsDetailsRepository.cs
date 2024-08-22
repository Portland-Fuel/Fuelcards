using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FcProspectsDetailsRepository : Repository<FcProspectsDetail>, IFcProspectsDetailsRepository
	{
		private readonly FuelcardsContext _db;

		public FcProspectsDetailsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcProspectsDetail source)
		{
			var dbObj = _db.FcProspectsDetails.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FcProspectsDetail source)
		{
			var dbObj = _db.FcProspectsDetails.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.FcProspectsDetails.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
       
        private void UpdateDbObject(FcProspectsDetail dbObj, FcProspectsDetail source)
		{
            dbObj.Id = source.Id;
            dbObj.TpChecled = source.TpChecled;
            dbObj.Notes = source.Notes;
            dbObj.PrimaryContactName = source.PrimaryContactName;
            dbObj.PrimaryContactNumber = source.PrimaryContactNumber;
        }
    }
}