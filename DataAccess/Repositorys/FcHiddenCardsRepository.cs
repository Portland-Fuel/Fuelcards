using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FcHiddenCardsRepository : Repository<FcHiddenCard>, IFcHiddenCardsRepository
    {
		private readonly FuelcardsContext _db;

		public FcHiddenCardsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcHiddenCard source)
		{
			var dbObj = _db.FcHiddenCards.FirstOrDefault(s => s.CardNo == source.CardNo);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

        //public FcMaskedCard Find(int code)
        //{
        //    return _db.FcMaskedCards.Find(code);
        //}
		public async Task UpdateAsync(FcHiddenCard source)
		{
			var dbObj = _db.FcHiddenCards.FirstOrDefault(e => e.CardNo == source.CardNo);
			if (dbObj is null) await _db.FcHiddenCards.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}

		public IQueryable<FcHiddenCard> Where(Func<FcHiddenCard, bool> predicate)
		{
			return _db.FcHiddenCards.Where(predicate).AsQueryable();
		}
		private void UpdateDbObject(FcHiddenCard dbObj, FcHiddenCard source)
		{
           dbObj.CardNo = source.CardNo;
           dbObj.PortlandId = source.PortlandId;
           dbObj.Network = source.Network;
			dbObj.AccountNo = source.AccountNo;
			dbObj.CostCentre = source.CostCentre;

        }
    }
}