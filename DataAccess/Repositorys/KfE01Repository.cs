using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Fuelcards;

namespace Portland.Data.Repository
{

	public class KfE01Repository : Repository<KfE1E3Transaction>, IKfE01Repository
	{
		private readonly FuelcardsContext _db;

		public KfE01Repository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE1E3Transaction source)
		{
			var dbObj = _db.KfE1E3Transactions.FirstOrDefault(s => s.TransactionId == source.TransactionId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

		public async Task UpdateAsync(KfE1E3Transaction source)
		{
			var dbObj = _db.KfE1E3Transactions.FirstOrDefault(s => s.TransactionId == source.TransactionId && s.ControlId == source.ControlId);
			if (dbObj is null) await _db.KfE1E3Transactions.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE1E3Transaction> Where(Func<KfE1E3Transaction, bool> predicate)
        {
            return _db.KfE1E3Transactions.Where(predicate).AsQueryable();
        }
        
        private void UpdateDbObject(KfE1E3Transaction dbObj, KfE1E3Transaction source)
		{

            dbObj.ControlId = source.ControlId;
            dbObj.PortlandId = source.PortlandId;
            dbObj.ReportType = source.ReportType;
            dbObj.TransactionNumber = source.TransactionNumber;
            dbObj.TransactionSequence = source.TransactionSequence;
            dbObj.TransactionType = source.TransactionType;
            dbObj.TransactionDate = source.TransactionDate;
            dbObj.TransactionTime = source.TransactionTime;
            dbObj.Period = source.Period;
            dbObj.SiteCode = source.SiteCode;
            dbObj.PumpNumber = source.PumpNumber;
            dbObj.CardNumber = source.CardNumber;
            dbObj.CustomerCode = source.CustomerCode;
            dbObj.CustomerAc = source.CustomerAc;
            dbObj.PrimaryRegistration = source.PrimaryRegistration;
            dbObj.Mileage = source.Mileage;
            dbObj.FleetNumber = source.FleetNumber;
            dbObj.ProductCode = source.ProductCode;
            dbObj.Quantity = source.Quantity;
            dbObj.Sign = source.Sign;
            dbObj.Cost = source.Cost;
            dbObj.CostSign = source.CostSign;
            dbObj.AccurateMileage = source.AccurateMileage;
            dbObj.CardRegistration = source.CardRegistration;
            dbObj.TransactonRegistration = source.TransactonRegistration;
            dbObj.Invoiced = source.Invoiced;
            dbObj.Commission = source.Commission;
            dbObj.InvoicePrice = source.InvoicePrice;
            dbObj.InvoiceNumber = source.InvoiceNumber;

        }
    }
}