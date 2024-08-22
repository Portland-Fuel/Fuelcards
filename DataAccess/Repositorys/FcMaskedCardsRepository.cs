using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FcMaskedCardsRepository : Repository<FcMaskedCard>, IFcMaskedCardsRepository
    {
		private readonly FuelcardsContext _db;

		public FcMaskedCardsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcMaskedCard source)
		{
			var dbObj = _db.FcMaskedCards.FirstOrDefault(s => s.CardNumber == source.CardNumber);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

        //public FcMaskedCard Find(int code)
        //{
        //    return _db.FcMaskedCards.Find(code);
        //}
		public async Task UpdateAsync(FcMaskedCard source)
		{
			var dbObj = _db.FcMaskedCards.FirstOrDefault(e => e.CardNumber == source.CardNumber);
			if (dbObj is null) await _db.FcMaskedCards.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
		//      public async Task<FcMaskedCard> FindAsync(int code)
		//      {
		//          var dbObj = await _db.FcMaskedCards.FindAsync(code);
		//          return dbObj;
		//      }

		public IQueryable<FcMaskedCard> Where(Func<FcMaskedCard, bool> predicate)
		{
			return _db.FcMaskedCards.Where(predicate).AsQueryable();
		}
		private void UpdateDbObject(FcMaskedCard dbObj, FcMaskedCard source)
		{
           dbObj.CardNumber = source.CardNumber;
           dbObj.PortlandId = source.PortlandId;
           dbObj.Network = source.Network;

        }
    }
}