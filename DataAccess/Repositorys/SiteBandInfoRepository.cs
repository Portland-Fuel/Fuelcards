using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class SiteBandInfoRepository : Repository<SiteBandInfo>, ISiteBandInfoRepository
    {
		private readonly FuelcardsContext _db;

		public SiteBandInfoRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(SiteBandInfo source)
		{
			var dbObj = _db.SiteBandInfos.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(SiteBandInfo source)
		{
			var dbObj = _db.SiteBandInfos.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.SiteBandInfos.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(SiteBandInfo dbObj, SiteBandInfo source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.EffectiveFrom = source.EffectiveFrom;
            dbObj.NetworkId = source.NetworkId;
			dbObj.Band = source.Band;
            dbObj.CommercialPrice = source.CommercialPrice;
        }
    }
}