using DataAccess.Cdata;
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using System.Runtime.CompilerServices;
using Fuelcards.Models;
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
        public List<CustomerPricingAddon>? GetListOfAddonsForCustomer(int PortlandId, EnumHelper.Network network)
        {
             List<CustomerPricingAddon>? AllCustomerAddons = GetAll().Where(e=>e.PortlandId == PortlandId && e.Network == (int)network).ToList();
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
        public List<FixedPriceContract>? AllFixContracts(int account)
        {
            List<FixedPriceContract>? fix =_db.FixedPriceContracts.Where(e=>e.FcAccount == account).ToList();
            return fix;
        }
        public Email AllEmail(int acccount)
        {
                FcEmail? email = _db.FcEmails.FirstOrDefault(e => e.Account == acccount);
                if (email is null) throw new Exception($"No Email found for Keyfuels customer on account {acccount}");
                Email emailModel = new()
                {
                    To = email.To,
                    Cc = email.Cc,
                    Bcc = email.Bcc,
                };
            return emailModel;
        }
        public EnumHelper.Network getNetworkFromAccount(int account)
        {
            int? networkId = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e=>e.FcAccountNo ==account)?.Network;
            return EnumHelper.NetworkEnumFromInt(networkId);   
        }
        public int[] GetAccounts(int portlandId)
        {
            int[]? AllAccounts = _db.FcNetworkAccNoToPortlandIds.Where(e=>e.PortlandId == portlandId).Select(e=>e.FcAccountNo).ToArray();
            return AllAccounts;
        }
    }
}
