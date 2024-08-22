using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class AllocatedVolumeSnapshotRepository : Repository<AllocatedVolumeTemp>, IAllocatedVolumeSnapshotRepository
    {
		private readonly FuelcardsContext _db;

		public AllocatedVolumeSnapshotRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(AllocatedVolumeTemp source)
		{
			var dbObj = _db.AllocatedVolumeTemps.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public void Delete(AllocatedVolumeTemp source)
        {
            var dbObj = _db.AllocatedVolumeTemps.FirstOrDefault(s => s.Id == source.Id);
            if (dbObj is null) return;
            _db.Remove(dbObj);
        }
        public void DeleteAll()
        {
            var dbObj = _db.AllocatedVolumeTemps.ToList();
            _db.AllocatedVolumeTemps.RemoveRange(dbObj);
        }
        public async Task UpdateAsync(AllocatedVolumeTemp source)
		{
			var dbObj = _db.AllocatedVolumeTemps.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.AllocatedVolumeTemps.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(AllocatedVolumeTemp dbObj, AllocatedVolumeTemp source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.TradeId = source.TradeId;
            dbObj.Volume = source.Volume;
			dbObj.AllocationId = source.AllocationId;
        }
    }
}