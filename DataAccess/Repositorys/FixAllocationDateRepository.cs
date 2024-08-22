using DataAccess.Fuelcards;
using DataAccess.Repository;
using DataAccess.Fuelcards;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository
{

	public class FixAllocationDateRepository : Repository<FixAllocationDate>, IFixAllocationDateRepository
    {
		private readonly FuelcardsContext _db;

		public FixAllocationDateRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FixAllocationDate source)
		{
			var dbObj = _db.FixAllocationDates.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FixAllocationDate source)
		{
			var dbObj = _db.FixAllocationDates.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.FixAllocationDates.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(FixAllocationDate dbObj, FixAllocationDate source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.Allocated = source.Allocated;
            dbObj.TradeId = source.TradeId;
            dbObj.NewAllocationDate = source.NewAllocationDate;
			dbObj.AllocationEnd = source.AllocationEnd;
        }
    }
}