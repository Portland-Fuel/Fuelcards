using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE20Repository : Repository<KfE20StoppedCard>, IKfE20Repository
    {
		private readonly FuelcardsContext _db;

		public KfE20Repository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE20StoppedCard source)
		{
			var dbObj = _db.KfE20StoppedCards.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(KfE20StoppedCard source)
		{
			var dbObj = _db.KfE20StoppedCards.FirstOrDefault(e=>e.Id == source.Id);
			if (dbObj is null) await _db.KfE20StoppedCards.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<KfE20StoppedCard> Where(Func<KfE20StoppedCard, bool> predicate)
        {
            return _db.KfE20StoppedCards.Where(predicate).AsQueryable();
        }
        public KfE20StoppedCard First(Func<KfE20StoppedCard, bool> predicate)
        {
            return (KfE20StoppedCard)_db.KfE20StoppedCards.First(predicate);
        }
        private void UpdateDbObject(KfE20StoppedCard dbObj, KfE20StoppedCard source)
		{

			dbObj.Id = dbObj.Id;
			dbObj.CardId = source.CardId;
			dbObj.PortlandId = source.CardId;
			dbObj.CustomerAccountCode = source.CardId;
			dbObj.CustomerAccountSuffix = source.CustomerAccountSuffix;
			dbObj.PanNumber = source.PanNumber;
			dbObj.Date = source.Date;
			dbObj.Time = source.Time;
			dbObj.StopCode = source.StopCode;
        }
    }
}