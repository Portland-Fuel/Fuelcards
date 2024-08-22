using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE22AccountsStoppedRepository : Repository<KfE22AccountsStopped>, IKfE22AccountsStoppedRepository
    {
		private readonly FuelcardsContext _db;

		public KfE22AccountsStoppedRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE22AccountsStopped source)
		{
			var dbObj = _db.KfE22AccountsStoppeds.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

        public KfE22AccountsStopped Find(int code)
        {
            return _db.KfE22AccountsStoppeds.Find(code);
        }
        public async Task UpdateAsync(KfE22AccountsStopped source)
		{
			var dbObj = _db.KfE22AccountsStoppeds.FirstOrDefault(e=>e.Id == source.Id);
			if (dbObj is null) await _db.KfE22AccountsStoppeds.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task<KfE22AccountsStopped> FindAsync(int code)
        {
            var dbObj = await _db.KfE22AccountsStoppeds.FindAsync(code);
            return dbObj;
        }

        public IQueryable<KfE22AccountsStopped> Where(Func<KfE22AccountsStopped, bool> predicate)
        {
            return _db.KfE22AccountsStoppeds.Where(predicate).AsQueryable();
        }
        public KfE22AccountsStopped First(Func<KfE22AccountsStopped, bool> predicate)
        {
            return _db.KfE22AccountsStoppeds.Where(predicate).First();
        }
        private void UpdateDbObject(KfE22AccountsStopped dbObj, KfE22AccountsStopped source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.CustomerAccountCode = source.CustomerAccountCode;
            dbObj.PortlandId = source.Id;
            dbObj.CustomerAccountSuffix = source.CustomerAccountSuffix;
            dbObj.Date = source.Date;
            dbObj.Time = source.Time;
            dbObj.StopStatusCode = source.StopStatusCode;
            dbObj.PersonRequestingStop = source.PersonRequestingStop;
            dbObj.StopReferenceNumber = source.StopReferenceNumber;
        }
    }
}