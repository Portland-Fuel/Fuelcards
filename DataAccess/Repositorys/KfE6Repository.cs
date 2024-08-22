using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE6Repository : Repository<KfE6Transfer>, IKfE6Repository
    {
		private readonly FuelcardsContext _db;

		public KfE6Repository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE6Transfer source)
		{
			var dbObj = _db.KfE6Transfers.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        

        public async Task UpdateAsync(KfE6Transfer source)
		{
			var dbObj = _db.KfE6Transfers.FirstOrDefault(e=>e.Id == source.Id);
			if (dbObj is null) await _db.KfE6Transfers.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE6Transfer> Where(Func<KfE6Transfer, bool> predicate)
        {
            return _db.KfE6Transfers.Where(predicate).AsQueryable();
        }
        public KfE6Transfer First(Func<KfE6Transfer, bool> predicate)
        {
            return _db.KfE6Transfers.Where(predicate).First();
        }
        private void UpdateDbObject(KfE6Transfer dbObj, KfE6Transfer source)
		{

			dbObj.Id = dbObj.Id;
			dbObj.TransactionNumber = source.TransactionNumber;
			dbObj.TransactionSequence = source.TransactionSequence;
			dbObj.ControlId = source.ControlId;

        }
    }
}