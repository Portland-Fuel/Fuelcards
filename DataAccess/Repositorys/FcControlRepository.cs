using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FcControlRepository : Repository<FcControl>, IFcControlRepository
    {
		private readonly FuelcardsContext _db;

		public FcControlRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcControl source)
		{
			var dbObj = _db.FcControls.FirstOrDefault(s => s.ControlId == source.ControlId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public FcControl MostRecentControlId()
        {
            return _db.FcControls.OrderByDescending(e => e.ControlId).FirstOrDefault();
        }

        public async Task UpdateAsync(FcControl source)
		{
			var dbObj = _db.FcControls.FirstOrDefault(s => s.BatchNumber == source.BatchNumber && s.TotalQuantity == source.TotalQuantity && s.CreationDate == source.CreationDate);
			if (dbObj is null) await _db.FcControls.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
		private void UpdateDbObject(FcControl dbObj, FcControl source)
		{

			dbObj.ControlId = dbObj.ControlId;
			dbObj.ReportType = source.ReportType;
			dbObj.BatchNumber = source.BatchNumber;
			dbObj.CreationDate = source.CreationDate;
			dbObj.CreationTime = source.CreationTime;
			dbObj.CustomerCode = source.CustomerCode;
			dbObj.CustomerAc = source.CustomerAc;
			dbObj.RecordCount = source.RecordCount;
			dbObj.TotalQuantity = source.TotalQuantity;
			dbObj.QuantitySign = source.QuantitySign;
			dbObj.TotalCost = source.TotalCost;
			dbObj.CostSign = source.CostSign;
			dbObj.Invoiced = source.Invoiced;
        }
    }
}