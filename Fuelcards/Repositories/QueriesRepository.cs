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
        public List<CustomerPricingAddon>? GetListOfAddonsForCustomer(int PortlandId)
        {
             List<CustomerPricingAddon>? AllCustomerAddons = GetAll().Where(e=>e.PortlandId == PortlandId).ToList();
            return AllCustomerAddons;
        }
        public int? GetPortlandIdFromXeroId(string xeroId)
        {
            var Return = _Cdb.PortlandIdToXeroIds.FirstOrDefault(e => e.XeroId == xeroId)?.PortlandId;
            return Return;
        }
        public IEnumerable<CustomerPricingAddon> GetAll()
        {
            return _db.CustomerPricingAddons.Where(e => e.Id > -1);
        }
        public int? GetPaymentTerms(string xeroId)
        {
            int? paymentTerms = _db.PaymentTerms.FirstOrDefault(e => e.XeroId == xeroId)?.PaymentTerms;
            return paymentTerms;
        }
        public List<FixedPriceContract>? AllFixContracts(int portlandId)
        {
            List<FixedPriceContract>? fix =_db.FixedPriceContracts.Where(e=>e.PortlandId == portlandId).ToList();
            return fix;
        }
    }
}
