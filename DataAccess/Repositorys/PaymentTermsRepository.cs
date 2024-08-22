using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class PaymentTermsRepository : Repository<PaymentTerm>, IPaymentTermsRepository
    {
		private readonly FuelcardsContext _db;

		public PaymentTermsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(PaymentTerm source)
		{
			var dbObj = _db.PaymentTerms.FirstOrDefault(s => s.XeroId == source.XeroId);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(PaymentTerm source)
		{
			var dbObj = _db.PaymentTerms.FirstOrDefault(s => s.XeroId == source.XeroId && s.Network == source.Network);
			if (dbObj is null) await _db.PaymentTerms.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        private void UpdateDbObject(PaymentTerm dbObj, PaymentTerm source)
		{
            dbObj.XeroId = source.XeroId;
            dbObj.PaymentTerms = source.PaymentTerms;

        }
    }
}