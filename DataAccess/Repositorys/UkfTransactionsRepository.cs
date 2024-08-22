using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class UkfTraansactionsRepository : Repository<UkfTransaction>, IUkfTransactionsRepository
	{
		private readonly FuelcardsContext _db;

		public UkfTraansactionsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(UkfTransaction source)
		{
			var dbObj = _db.UkfTransactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(UkfTransaction source)
		{
			var dbObj = _db.UkfTransactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) await _db.UkfTransactions.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<UkfTransaction> Where(Func<UkfTransaction, bool> predicate)
        {
            return _db.UkfTransactions.Where(predicate).AsQueryable();
        }
        private void UpdateDbObject(UkfTransaction dbObj, UkfTransaction source)
		{
            dbObj.TransactionId = source.TransactionId;
            dbObj.ControlId = source.ControlId;
            dbObj.PortlandId = source.PortlandId;
            dbObj.Batch = source.Batch;
            dbObj.Division = source.Division;
            dbObj.ClientType = source.ClientType;
            dbObj.Customer = source.Customer;
            dbObj.Site = source.Site;
            dbObj.TranDate = source.TranDate;
            dbObj.TranTime = source.TranTime;
            dbObj.CardNo = source.CardNo;
            dbObj.Registration = source.Registration;
            dbObj.Mileage = source.Mileage;
            dbObj.Quantity = source.Quantity;
            dbObj.ProdNo = source.ProdNo;
            dbObj.MonthNo = source.MonthNo;
            dbObj.WeekNo = source.WeekNo;
            dbObj.TranNoItem = source.TranNoItem;
            dbObj.Price = source.Price;
            dbObj.PanNumber = source.PanNumber;
            dbObj.Invoiced = source.Invoiced;
            dbObj.ReceiptNo = source.ReceiptNo;
            dbObj.Commission = source.Commission;
            dbObj.InvoicePrice = source.InvoicePrice;
            dbObj.InvoiceNumber = source.InvoiceNumber;

        }
    }
}