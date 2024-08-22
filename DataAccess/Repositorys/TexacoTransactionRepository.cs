using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class TexacoTransactionRepository : Repository<TexacoTransaction>, ITexacoTracactionRepository
	{
		private readonly FuelcardsContext _db;

		public TexacoTransactionRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(TexacoTransaction source)
		{
			var dbObj = _db.TexacoTransactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(TexacoTransaction source)
		{
			var dbObj = _db.TexacoTransactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) await _db.TexacoTransactions.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate)
        {
            return _db.TexacoTransactions.Where(predicate).AsQueryable();
        }
        private void UpdateDbObject(TexacoTransaction dbObj, TexacoTransaction source)
		{
            dbObj.TransactionId = dbObj.TransactionId;
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
            dbObj.IsoNumber = source.IsoNumber;
            dbObj.Invoiced = source.Invoiced;
            dbObj.Commission = source.Commission;
            dbObj.InvoicePrice = source.InvoicePrice;
            dbObj.InvoiceNumber = source.InvoiceNumber;
        }
    }
}