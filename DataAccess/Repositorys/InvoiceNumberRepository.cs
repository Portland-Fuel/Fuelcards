using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

    public class InvoiceNumberRepository : Repository<InvoiceNumber>, IInvoiceNumberRepository
    {
        private readonly FuelcardsContext _db;

        public InvoiceNumberRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InvoiceNumber source)
        {
            var dbObj = _db.InvoiceNumbers.FirstOrDefault(s => s.InvoiceNumber1 == source.InvoiceNumber1);
            if (dbObj is null) _db.Add(source);
            else UpdateDbObject(dbObj, source);
        }
        public async Task UpdateAsync(InvoiceNumber source)
        {
            var dbObj = _db.InvoiceNumbers.FirstOrDefault(s => s.InvoiceNumber1 == source.InvoiceNumber1);
            if (dbObj is null) await _db.InvoiceNumbers.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
        private void UpdateDbObject(InvoiceNumber dbObj, InvoiceNumber source)
        {
            dbObj.Network = dbObj.Network;
            dbObj.InvoiceNumber1 = source.InvoiceNumber1;
        }
    }
}