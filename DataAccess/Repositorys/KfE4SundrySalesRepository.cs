using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

    public class KfE4SundrySalesRepository : Repository<KfE4SundrySale>, IKfE4SundrySales
    {
		private readonly FuelcardsContext _db;

		public KfE4SundrySalesRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE4SundrySale source)
		{
			var dbObj = _db.KfE4SundrySales.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public KfE4SundrySale First(Func<KfE4SundrySale, bool> predicate)
        {
            return _db.KfE4SundrySales.Where(predicate).First();
        }

        public async Task UpdateAsync(KfE4SundrySale source)
		{
			var dbObj = _db.KfE4SundrySales.FirstOrDefault(e=>e.Id == source.Id);
			if (dbObj is null) await _db.KfE4SundrySales.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE4SundrySale> Where(Func<KfE4SundrySale, bool> predicate)
        {
            return _db.KfE4SundrySales.Where(predicate).AsQueryable();
        }
        private void UpdateDbObject(KfE4SundrySale dbObj, KfE4SundrySale source)
		{
			dbObj.TransactionNumber = source.TransactionNumber;
			dbObj.TransactionSequence = source.TransactionSequence;
			dbObj.TransactionType = source.TransactionType;
			dbObj.TransactionDate = source.TransactionDate;
			dbObj.TransactionTime = source.TransactionTime;
			dbObj.CustomerCode = source.CustomerCode;
			dbObj.CustomerAc = source.CustomerAc;
			dbObj.Period = source.Period;
			dbObj.ProductCode = source.ProductCode;
			dbObj.Quantity = source.Quantity;
			dbObj.QuantitySign = source.QuantitySign;
			dbObj.Value = source.Value;
			dbObj.ValueSign = source.ValueSign;
			dbObj.CardNumber = source.CardNumber;
			dbObj.VehicleRegistration = source.VehicleRegistration;
			dbObj.Reference = source.Reference;
			dbObj.Invoiced = source.Invoiced;
			dbObj.ControlId = source.ControlId;
        }
    }
}