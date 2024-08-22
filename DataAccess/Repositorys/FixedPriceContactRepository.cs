using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class FixedPriceContractsRepository : Repository<FixedPriceContract>, IFixedPriceContractsRepository
    {
		private readonly FuelcardsContext _db;

		public FixedPriceContractsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FixedPriceContract source)
		{
			var dbObj = _db.FixedPriceContracts.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FixedPriceContract source)
		{
			var dbObj = _db.FixedPriceContracts.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.FixedPriceContracts.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        //public IQueryable<TransactionalSite> Where(Func<TransactionalSite, bool> predicate)
        //{
        //    return _db.TransactionalSites.Where(predicate).AsQueryable();
        //}
        private void UpdateDbObject(FixedPriceContract dbObj, FixedPriceContract source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.PortlandId = source.PortlandId;
            dbObj.EffectiveFrom = source.EffectiveFrom;
            dbObj.EndDate = source.EndDate;
            dbObj.FixedPrice = source.FixedPrice;
            dbObj.FixedPriceIncDuty = source.FixedPriceIncDuty;
            dbObj.FixedVolume = source.FixedVolume;
            dbObj.Period = source.Period;
            dbObj.Network = source.Network;
            dbObj.TradeReference = source.TradeReference;
            dbObj.TerminationDate = source.TerminationDate;
            dbObj.Grade = source.Grade;
            dbObj.FrequencyId = source.FrequencyId;
            dbObj.FcAccount = source.FcAccount;
            
        }
    }
}