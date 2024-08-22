using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FgTransactionRepository : Repository<FgTransaction>, IFgTransactionRepository
    {
		private readonly FuelcardsContext _db;

		public FgTransactionRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FgTransaction source)
		{
			var dbObj = _db.FgTransactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        

        public async Task UpdateAsync(FgTransaction source)
		{
			var dbObj = _db.FgTransactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) await _db.FgTransactions.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
		private void UpdateDbObject(FgTransaction dbObj, FgTransaction source)
		{
            dbObj.TransactionId = dbObj.TransactionId;
            dbObj.FileProcessDate = dbObj.FileProcessDate;
            dbObj.MerchantId = dbObj.MerchantId;
            dbObj.MerchantName = dbObj.MerchantName;
            dbObj.Supermarket = dbObj.Supermarket;
            dbObj.TransactionDate = dbObj.TransactionDate;
            dbObj.TransactionTime = dbObj.TransactionTime;
            dbObj.EftNumber = dbObj.EftNumber;
            dbObj.CustomerNumber = dbObj.CustomerNumber;
            dbObj.PanNumber = dbObj.PanNumber;
            dbObj.CardName = dbObj.CardName;
            dbObj.RegNo = dbObj.RegNo;
            dbObj.Mileage = dbObj.Mileage;
            dbObj.ProductNumber = dbObj.ProductNumber;
            dbObj.ProductName = dbObj.ProductName;
            dbObj.ProductCode = dbObj.ProductCode;
            dbObj.ProductQuantity = dbObj.ProductQuantity;
            dbObj.GrossAmount = dbObj.GrossAmount;
            dbObj.PurchaseRefund = dbObj.PurchaseRefund;
            dbObj.NetAmount = dbObj.NetAmount;
            dbObj.VatAmount = dbObj.VatAmount;
            dbObj.VatRate = dbObj.VatRate;
            dbObj.AuthCode = dbObj.AuthCode;
            dbObj.PortlandId = dbObj.PortlandId;
            dbObj.Invoiced = source.Invoiced;
        }
    }
}