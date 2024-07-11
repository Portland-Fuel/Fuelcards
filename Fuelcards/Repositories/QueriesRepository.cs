using DataAccess.Cdata;
using DataAccess.Fuelcards;
using System.Runtime.CompilerServices;

namespace Fuelcards.Repositories
{
    public class QueriesRepository: IQueriesRepository
    {
        private readonly FuelcardsContext _db;
        private readonly CDataContext _Cdb;
        public QueriesRepository(FuelcardsContext db, CDataContext cDb)
        {
            _db = db;
            _Cdb = cDb;
        }
        public void GetListOfAddonsForCustomer(int PortlandId)
        {
             List<CustomerPricingAddon>? AllCustomerAddons = GetAll().Where(e=>e.PortlandId == PortlandId).ToList();
        }
        public int? GetPortlandIdFromXeroId(string xeroId)
        {
            return _Cdb.PortlandIdToXeroIds.FirstOrDefault(e => e.XeroId == xeroId)?.PortlandId;
        }
        public IEnumerable<CustomerPricingAddon> GetAll()
        {
            return _db.CustomerPricingAddons.Where(e => e.Id > -1);
        }
    }
}
