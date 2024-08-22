using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class MissingProductsRepository : Repository<MissingProductValue>, IMissingProductsRepository
    {
		private readonly FuelcardsContext _db;

		public MissingProductsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(MissingProductValue source)
		{
			var dbObj = _db.MissingProductValues.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

		public async Task UpdateAsync(MissingProductValue source)
		{
			var dbObj = _db.MissingProductValues.FirstOrDefault(e => e.Id == source.Id);
			if (dbObj is null) await _db.MissingProductValues.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}

		
		private void UpdateDbObject(MissingProductValue dbObj, MissingProductValue source)
		{
           dbObj.Id = dbObj.Id;
           dbObj.Network = source.Network;
           dbObj.Product = source.Product;
			dbObj.Value = source.Value;
        }
    }
}