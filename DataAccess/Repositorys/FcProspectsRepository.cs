using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FcProspectsRepository : Repository<FcProspect>, IFcProspectsRepository
	{
		private readonly FuelcardsContext _db;

		public FcProspectsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcProspect source)
		{
			var dbObj = _db.FcProspects.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FcProspect source)
		{
			var dbObj = _db.FcProspects.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.FcProspects.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
       
        private void UpdateDbObject(FcProspect dbObj, FcProspect source)
		{
            dbObj.Id = source.Id;
            dbObj.Name = source.Name;

        }
    }
}