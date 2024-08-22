using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE11ProductsRepository : Repository<KfE11Product>, IKfE11ProductsRepository
    {
		private readonly FuelcardsContext _db;

		public KfE11ProductsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE11Product source)
		{
			var dbObj = _db.KfE11Products.FirstOrDefault(s => s.ProductCode == source.ProductCode);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

        public async Task<KfE11Product> FindAsync(int productCode)
        {
            return await _db.KfE11Products.FindAsync(productCode);
        }
       
        public async Task UpdateAsync(KfE11Product source)
		{
			var dbObj = _db.KfE11Products.FirstOrDefault(e=>e.ProductCode == source.ProductCode);
			if (dbObj is null) await _db.KfE11Products.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public KfE11Product Find(int productcode)
        {
            return _db.KfE11Products.Find(productcode);
        }
        public IQueryable<KfE11Product> Where(Func<KfE11Product, bool> predicate)
        {
            return _db.KfE11Products.Where(predicate).AsQueryable();
        }
        private void UpdateDbObject(KfE11Product dbObj, KfE11Product source)
		{

			dbObj.ProductCode = dbObj.ProductCode;
			dbObj.ProductDescription = source.ProductDescription;

        }
    }
}