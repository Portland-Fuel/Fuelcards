using DataAccess.Fuelcards;using DataAccess.Repository;

using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class InvoiceFormatGroupsRepository : Repository<InvoiceFormatGroup>, IInvoiceFormatGroupsRepository
    {
		private readonly FuelcardsContext _db;

		public InvoiceFormatGroupsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}
        
		public void Update(InvoiceFormatGroup source)
		{
			var dbObj = _db.InvoiceFormatGroups.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(InvoiceFormatGroup source)
		{
			var dbObj = _db.InvoiceFormatGroups.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.InvoiceFormatGroups.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        
        private void UpdateDbObject(InvoiceFormatGroup dbObj, InvoiceFormatGroup source)
		{
            dbObj.Id = dbObj.Id;
			dbObj.Group = source.Group;
        }
    }
}