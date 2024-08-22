using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE5Repository : Repository<KfE5Stock>, IKfE5Repository
    {
		private readonly FuelcardsContext _db;

		public KfE5Repository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE5Stock source)
		{
			var dbObj = _db.KfE5Stocks.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        

        public async Task UpdateAsync(KfE5Stock source)
		{
			var dbObj = _db.KfE5Stocks.FirstOrDefault(e=>e.Id == source.Id);
			if (dbObj is null) await _db.KfE5Stocks.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE5Stock> Where(Func<KfE5Stock, bool> predicate)
        {
            return _db.KfE5Stocks.Where(predicate).AsQueryable();
        }
        public KfE5Stock First(Func<KfE5Stock, bool> predicate)
        {
            return _db.KfE5Stocks.First(predicate);
        }
        private void UpdateDbObject(KfE5Stock dbObj, KfE5Stock source)
		{

			dbObj.Id = dbObj.Id;
            dbObj.CustomerCode = source.CustomerCode;
            dbObj.CustomerAc = source.CustomerAc;
            dbObj.ProductCode = source.ProductCode;
            dbObj.OpeningStockBalance = source.OpeningStockBalance;
            dbObj.OpeningBalanceSign = source.OpeningBalanceSign;
            dbObj.DrawingQuantity = source.DrawingQuantity;
            dbObj.DrawingQuantitySign = source.DrawingQuantitySign;
            dbObj.NumberOfDrawings = source.NumberOfDrawings;
            dbObj.DeliveryQuantity = source.DeliveryQuantity;
            dbObj.DeliveryQuantitySign = source.DeliveryQuantitySign;
            dbObj.NumberOfDeliveries = source.NumberOfDeliveries;
            dbObj.ClosingStockBalance = source.ClosingStockBalance;
            dbObj.ClosingBalanceSign = source.ClosingBalanceSign;
        }
    }
}