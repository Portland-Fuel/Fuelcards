using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE2DeliveryRepository : Repository<KfE2Delivery>, IKfE2DeliveryRepository
    {
		private readonly FuelcardsContext _db;

		public KfE2DeliveryRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE2Delivery source)
		{
			var dbObj = _db.KfE2Deliveries.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        

        public async Task UpdateAsync(KfE2Delivery source)
		{
			var dbObj = _db.KfE2Deliveries.FirstOrDefault(e=>e.Id == source.Id);
			if (dbObj is null) await _db.KfE2Deliveries.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE2Delivery> Where(Func<KfE2Delivery, bool> predicate)
        {
            return _db.KfE2Deliveries.Where(predicate).AsQueryable();
        }
        private void UpdateDbObject(KfE2Delivery dbObj, KfE2Delivery source)
		{

			dbObj.TransactionNumber = source.TransactionNumber;
			dbObj.TransactionSequence = source.TransactionSequence;
			dbObj.TransactionType = source.TransactionType;
			dbObj.TransactionDate = source.TransactionDate;
			dbObj.TransactionTime = source.TransactionTime;
			dbObj.Period = source.Period;
			dbObj.SiteCode = source.SiteCode;
			dbObj.CustomerCode = source.CustomerCode;
			dbObj.CustomerAc = source.CustomerAc;
			dbObj.SupplierName = source.SupplierName;
			dbObj.ProductCode = source.ProductCode;
        }
    }
}