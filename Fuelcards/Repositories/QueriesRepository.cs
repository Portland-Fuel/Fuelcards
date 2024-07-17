using DataAccess.Cdata;
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using System.Runtime.CompilerServices;
using Fuelcards.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Fuelcards.Controllers;
namespace Fuelcards.Repositories
{
    public class QueriesRepository : IQueriesRepository
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
            List<CustomerPricingAddon>? AllCustomerAddons = GetAll().Where(e => e.PortlandId == PortlandId && e.Network == (int)network).ToList();
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
            List<FixedPriceContract>? fix = _db.FixedPriceContracts.Where(e => e.FcAccount == account).ToList();
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
            int? networkId = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e => e.FcAccountNo == account)?.Network;
            return EnumHelper.NetworkEnumFromInt(networkId);
        }
        public int[] GetAccounts(int portlandId)
        {
            int[]? AllAccounts = _db.FcNetworkAccNoToPortlandIds.Where(e => e.PortlandId == portlandId).Select(e => e.FcAccountNo).ToArray();
            return AllAccounts;
        }
        public double? GetBasePrice(DateOnly invoiceDate)
        {
            return _db.FuelcardBasePrices.FirstOrDefault(e => e.EffectiveFrom <= invoiceDate && e.EffectiveTo >= invoiceDate)?.BasePrice;
        }
        public int? GetTotalEDIs(int network)
        {
            int? NumberOfImports = _db.FcControls.Where(e => e.Invoiced != true && e.Network == network).Count();
            return NumberOfImports;
        }
        public double? GetDieselBand7Texaco()
        {
            List<int?> Band7 = _db.SiteNumberToBands.Where(e => e.Band == "7").Select(e => e.SiteNumber).ToList();
            List<int> controlIds = _db.FcControls.Where(e => e.Invoiced != true && e.Network == 2).Select(e => e.ControlId).ToList();
            double? quantity = (_db.TexacoTransactions.Where(e => controlIds.Contains((int)e.ControlId) && e.ProdNo == 1 && Band7.Contains((int?)e.Site)).Sum(e => e.Quantity)) / 100;
            return quantity;
        }
        public List<CustomerInvoice>? GetCustomersToInvoice(int network, DateOnly invoiceDate)
        {
            int tennant = 0;
            List<CustomerInvoice> CustomersToInvoiceOnNetwork = new();
            var CustomersAccounts = GetAllFixedCustomers(invoiceDate, network);
            List<int> fcControls = _db.FcControls.Where(e => e.Invoiced != true).Select(e => e.ControlId).ToList();
            List<int> accounts = new();
            switch (network)
            {
                case 0:
                    accounts = _db.KfE1E3Transactions.Where(e => fcControls.Contains(e.ControlId)).Select(e => (int)e.CustomerCode).Distinct().ToList();
                    break;
                case 1:
                    accounts = _db.UkfTransactions.Where(e => fcControls.Contains(e.ControlId)).Select(e => (int)e.Customer).ToList();
                    break;
                case 2:
                    accounts = _db.TexacoTransactions.Where(e => fcControls.Contains((int)e.ControlId)).Select(e => (int)e.Customer).ToList();
                    break;
                case 3:
                    tennant = 1;
                    break;
            }
            foreach (var accountNumber in accounts)
            {
                int? PortlandId = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e => e.Network == network && e.FcAccountNo == accountNumber)?.PortlandId;
                if (PortlandId == null) throw new ArgumentException($"Portland Id could not be established from the acocunt number {accountNumber}");
                string? xeroId = _Cdb.PortlandIdToXeroIds.FirstOrDefault(e => e.PortlandId == PortlandId && e.XeroTennant == tennant)?.XeroId;
                if (xeroId == null) throw new ArgumentException($"Xero Id could not be established from the Portland Id {PortlandId}");
                var name = HomeController.PFLXeroCustomersData.Where(e => e.ContactID.ToString() == xeroId).Select(e=>e.Name).ToList();
                if (name == null) throw new ArgumentException($"Customer name could not be established from the Xero Id {xeroId}");
                CustomerInvoice model = new()
                {
                    name = name[0].ToString(),
                    addon = _db.CustomerPricingAddons.FirstOrDefault(e => e.Network == network && e.PortlandId == PortlandId)?.Addon
                };
                CustomersToInvoiceOnNetwork.Add(model);
            }
            return CustomersToInvoiceOnNetwork;
        }
        public List<int>? GetAllFixedCustomers(DateOnly InvoiceDate, int network)
        {
            List<int> Customers = new();
            var TradeIds = _db.FixedPriceContracts.Where(e => e.EndDate.Value.AddDays(8) >= InvoiceDate && e.TerminationDate == null && e.Network[0] == network);
            if (TradeIds is null) return null;
            foreach (var item in TradeIds)
            {
                var Account = (int)item.FcAccount;
                Customers.Add(Account);

            }
            return Customers;
        }
    }
}
