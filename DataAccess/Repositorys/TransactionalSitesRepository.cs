using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class TransactionalSitesRepository : Repository<TransactionalSite>, ITransactionalSitesRepository
    {
		private readonly FuelcardsContext _db;

		public TransactionalSitesRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(TransactionalSite source)
		{
			var dbObj = _db.TransactionalSites.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(TransactionalSite source)
		{
			var dbObj = _db.TransactionalSites.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.TransactionalSites.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        //public IQueryable<TransactionalSite> Where(Func<TransactionalSite, bool> predicate)
        //{
        //    return _db.TransactionalSites.Where(predicate).AsQueryable();
        //}
        private void UpdateDbObject(TransactionalSite dbObj, TransactionalSite source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.SiteCode = source.SiteCode;
            dbObj.Network = source.Network;
            dbObj.EffectiveDate = source.EffectiveDate;
        }
    }
}