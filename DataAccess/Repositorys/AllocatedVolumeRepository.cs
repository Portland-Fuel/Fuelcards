using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class AllocatedVolumeRepository : Repository<AllocatedVolume>, IAllocatedVolumeRepository
    {
		private readonly FuelcardsContext _db;

		public AllocatedVolumeRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(AllocatedVolume source)
		{
			var dbObj = _db.AllocatedVolumes.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public void Delete(AllocatedVolume source)
        {
            var dbObj = _db.AllocatedVolumes.FirstOrDefault(s => s.Id == source.Id);
            if (dbObj is null) return;
            _db.Remove(dbObj);
        }
        public void DeleteAll()
        {
            var dbObj = _db.AllocatedVolumes.ToList();
            _db.AllocatedVolumes.RemoveRange(dbObj);
        }
        public async Task UpdateAsync(AllocatedVolume source)
		{
			var dbObj = _db.AllocatedVolumes.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.AllocatedVolumes.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(AllocatedVolume dbObj, AllocatedVolume source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.TradeId = source.TradeId;
            dbObj.Volume = source.Volume;
			dbObj.AllocationId = source.AllocationId;
        }
    }
}