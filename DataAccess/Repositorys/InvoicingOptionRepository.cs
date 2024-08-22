using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class InvoicingOptionsRepository : Repository<InvoicingOption>, IInvoicingOptionsRepository
	{
		private readonly FuelcardsContext _db;

		public InvoicingOptionsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}
        
		public void Update(InvoicingOption source)
		{
			var dbObj = _db.InvoicingOptions.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(InvoicingOption source)
		{
			var dbObj = _db.InvoicingOptions.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.InvoicingOptions.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        
        private void UpdateDbObject(InvoicingOption dbObj, InvoicingOption source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.PortlandId = source.PortlandId;
            dbObj.GroupedNetwork = source.GroupedNetwork;
			dbObj.Displaygroup = source.Displaygroup; 
        }
    }
}