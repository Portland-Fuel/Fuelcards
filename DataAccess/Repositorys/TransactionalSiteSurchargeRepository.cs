using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class TransactionalSiteSurchargeRepository : Repository<TransactionSiteSurcharge>, ITransactionalSiteSurchargeRepository
	{
		private readonly FuelcardsContext _db;

		public TransactionalSiteSurchargeRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(TransactionSiteSurcharge source)
		{
			var dbObj = _db.TransactionSiteSurcharges.FirstOrDefault(s => s.EffectiveDate == source.EffectiveDate && s.Network == source.Network);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(TransactionSiteSurcharge source)
		{
			var dbObj = _db.TransactionSiteSurcharges.FirstOrDefault(s => s.EffectiveDate == source.EffectiveDate && s.Network == source.Network);
			if (dbObj is null) await _db.TransactionSiteSurcharges.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        //public IQueryable<TexacoTransaction> Where(Func<TexacoTransaction, bool> predicate)
        //{
        //    return _db.TexacoTransactions.Where(predicate).AsQueryable();
        //}
        private void UpdateDbObject(TransactionSiteSurcharge dbObj, TransactionSiteSurcharge source)
		{
            dbObj.EffectiveDate = source.EffectiveDate;
            dbObj.Network = source.Network;
            dbObj.Surcharge = source.Surcharge;
			dbObj.ChargeType = source.ChargeType;


        }
    }
}