using DataAccess.Cdata;
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using System.Runtime.CompilerServices;
using Fuelcards.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Fuelcards.Controllers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xero.NetStandard.OAuth2.Models;
using static Fuelcards.Models.CustomerDetailsModels;
using System.Linq;
using Fuelcards.Models;
using Xero.NetStandard.OAuth2.Client;
using Fuelcards.InvoiceMethods;
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
            List<CustomerInvoice> test = new();
            return test;
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
        public void UpdateAddon(CustomerDetailsModels.AddonFromJs data)
        {
            try
            {
                ProcessAddonList(data.keyFuels);
                ProcessAddonList(data.uKFuels);
                ProcessAddonList(data.texaco);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        private void ProcessAddonList(List<AddonDataFromJS>? addonList)
        {
            if (addonList == null) return;

            foreach (var addon in addonList)
            {
                CustomerPricingAddon model = new();

                model.EffectiveDate = DateOnly.Parse(addon.effectiveFrom);
                if (model.EffectiveDate == null)
                    throw new ArgumentException("Problem mapping the effective date from the Json object to the CustomerPricingAddon model");

                model.Addon = Convert.ToDouble(addon.addon);
                if (model.Addon == null)
                    throw new ArgumentException("Problem converting the addon from a string to a double");

                var networkEntry = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e => e.FcAccountNo == Convert.ToInt32(addon.account));
                if (networkEntry == null)
                    throw new ArgumentException($"Could not pull out the network and Portland ID for the following account number {addon.account}");

                model.Network = (int)networkEntry.Network;
                model.PortlandId = (int)networkEntry.PortlandId;

                _db.CustomerPricingAddons.Add(model);
                _db.SaveChanges();
            }
        }
        public List<int> GetFailedSiteBanding(int network)
        {
            List<int> Failed = new();
            List<int?> Sites = new();
            List<int> fcControls = _db.FcControls.Where(e => e.Invoiced != true).Select(e => e.ControlId).ToList();
            switch (network)
            {
                case 0:
                    Sites = _db.KfE1E3Transactions.Where(e => fcControls.Contains(e.ControlId)).Select(e => e.SiteCode).Distinct().ToList();
                    break;
                case 1:
                    Sites = _db.UkfTransactions.Where(e => fcControls.Contains(e.ControlId)).Select(e => (int?)e.Site).Distinct().ToList();
                    break;
                case 2:
                    Sites = _db.TexacoTransactions.Where(e => fcControls.Contains((int)e.ControlId)).Select(e => (int?)e.Site).Distinct().ToList();
                    break;
                case 3:
                    break;
            }
            foreach (var item in Sites)
            {
                SiteNumberToBand? SiteInfo = _db.SiteNumberToBands.FirstOrDefault(e => e.SiteNumber == item);
                if (SiteInfo is null)
                {
                    Failed.Add((int)item);
                    continue;
                }
                if (SiteInfo.Band is null) Failed.Add((int)item);
            }
            return Failed;
        }
        public double? GetProductVolume(EnumHelper.Products product)
        {
            double? TotalQuantity = null;
            switch (product)
            {
                case EnumHelper.Products.Diesel:
                    List<int> DieselProductCodes = new List<int> { 1,70 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && DieselProductCodes.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity) / 100;
                    return TotalQuantity;
                case EnumHelper.Products.Gasoil:
                    break;
                case EnumHelper.Products.ULSP:
                    List<int> UnleadedProductCodes = new List<int>{ 2 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && UnleadedProductCodes.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity)/100;
                    return TotalQuantity;

                case EnumHelper.Products.Lube:
                    break;
                case EnumHelper.Products.Adblue:
                    List<int> AdblueProductCodes = new List<int> { 8,18 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && AdblueProductCodes.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity) / 100;
                    return TotalQuantity;
                case EnumHelper.Products.SuperUnleaded:
                    List<int> SuperUnleaded = new List<int> { 3 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && SuperUnleaded.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity) / 100;
                    return TotalQuantity;
                case EnumHelper.Products.TescoDieselNewDiesel:
                    break;
                case EnumHelper.Products.PackagedAdblue:
                    break;
                case EnumHelper.Products.PremiumDiesel:
                    List<int> PremiumDiesel = new List<int> { 30 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && PremiumDiesel.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity) / 100;
                    return TotalQuantity;
                
            }
            return null;
        }
        public async Task<IEnumerable<KfE1E3Transaction>> GetAllKeyfuelTransactionsThatNeedToBeInvoiced(DateOnly InvoiceDate)
        {
            
            var controlIds = _db.FcControls.Where(e => e.CreationDate <= InvoiceDate && e.Invoiced != true)
                .OrderByDescending(e => e.ControlId)
                .Select(e => e.ControlId)
                .ToList();

            var TransactionsToUse = _db.KfE1E3Transactions.Where(e => e.Invoiced != true || controlIds.Contains(e.ControlId)).ToList();
            var PotentialSundrySales = _db.KfE4SundrySales.Where(e => e.Invoiced != true);
            if (PotentialSundrySales?.Any() == true)
            {
                // Ensure transactionsToUse is a List (if it's not already)
                var editableTransactions = TransactionsToUse as List<KfE1E3Transaction> ?? TransactionsToUse.ToList();
                foreach (var item in PotentialSundrySales)
                {
                    editableTransactions.Add(ConvertSundryToTransaction(item));
                }
                TransactionsToUse = editableTransactions;
            }
            return TransactionsToUse;
        }

        public async Task<IEnumerable<UkfTransaction>> GetAllUKTransactionsThatNeedToBeInvoiced(DateOnly InvoiceDate)
        {
            var controlIds = _db.FcControls.Where(e => e.CreationDate < InvoiceDate.AddDays(1) && e.Invoiced != true)
                 .OrderByDescending(e => e.ControlId)
                 .Select(e => e.ControlId)
                 .ToList();
            var TransactionsToUse = _db.UkfTransactions.Where(e => controlIds.Contains(e.ControlId) || e.Invoiced != true);
            return TransactionsToUse;
        }
        public async Task<IEnumerable<TexacoTransaction>> GetAllTexTransactionsThatNeedToBeInvoiced(DateOnly InvoiceDate)
        {
            var controlIds = _db.FcControls.Where(e => e.CreationDate < InvoiceDate.AddDays(1) && e.Invoiced != true)
                .OrderByDescending(e => e.ControlId)
                .Select(e => e.ControlId)
                .ToList();
            var Transactions = _db.TexacoTransactions.Where(e => e.Invoiced != true || controlIds.Contains((int)e.ControlId));
            return Transactions;
        }
        public int? GetPortlandIdFromAccount(int account)
        {
            int? portlandId = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e => e.FcAccountNo == account)?.PortlandId;
            return portlandId;
        }
        public KfE1E3Transaction ConvertSundryToTransaction(KfE4SundrySale sundrysale)
        {

            KfE1E3Transaction model = new()
            {
                AccurateMileage = null,
                CardNumber = sundrysale.CardNumber,
                CardRegistration = null,
                Commission = null,
                ControlId = (int)sundrysale.ControlId,
                CostSign = null,
                Cost = sundrysale.Value,
                CustomerAc = sundrysale.CustomerAc,
                CustomerCode = sundrysale.CustomerCode,
                FleetNumber = null,
                Invoiced = sundrysale.Invoiced,
                InvoiceNumber = null,
                InvoicePrice = null,
                Mileage = null,
                Period = sundrysale.Period,
                PortlandId = GetPortlandIdFromAccount((int)sundrysale.CustomerAc),
                PrimaryRegistration = null,
                ProductCode = sundrysale.ProductCode,
                PumpNumber = null,
                Quantity = sundrysale.Quantity,
                ReportType = null,
                Sign = null,
                SiteCode = null,
                TransactionDate = sundrysale.TransactionDate,
                TransactionTime = sundrysale.TransactionTime,
                TransactionId = sundrysale.Id,
                TransactionNumber = sundrysale.TransactionNumber,
                TransactionSequence = sundrysale.TransactionSequence,
                TransactionType = sundrysale.TransactionType,
                TransactonRegistration = sundrysale.VehicleRegistration,


            };
            return model;

        }

        public List<List<GenericTransactionFile>> GetUninvoicedTransactionsAndGroupThemByCustomerAndNetwork(DateOnly InvoiceDate, DateOnly FuelgenieDate)
        {
            List<GenericTransactionFile> AllTransactions;
            bool updatesMade;
            do
            {
                updatesMade = false;

                AllTransactions = Transactions.TurnTransactionIntoGeneric(
                    GetAllKeyfuelTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(),
                    GetAllUKTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(),
                    GetAllTexTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(),
                    GetAllFGTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList());

                foreach (var item in AllTransactions.Where(e => e.PortlandId is null))
                {
                    try
                    {
                        var PortlandId = GetPortlandIdFromMaskedCards(item.CardNumber);
                        if (PortlandId is not null)
                        {
                            UpdatePortlandIdOnTransaction(item, PortlandId);
                            updatesMade = true;
                        }
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"error with a masked card. The card number {item.CardNumber} Failed.");
                    }
                }
            } while (updatesMade);
            List<List<GenericTransactionFile>> TransactionsByCustomerAndNetwork = GroupTransactionsByCustomer(AllTransactions);
            return TransactionsByCustomerAndNetwork;
        }

        public int? GetPortlandIdFromMaskedCards(decimal? cardNumber)
        {
            try
            {
                return _db.FcMaskedCards.FirstOrDefault(e => e.CardNumber == cardNumber).PortlandId;
            }
            catch (Exception)
            {
                return _db.FcHiddenCards.FirstOrDefault(e => cardNumber.Value.ToString().Contains(e.CardNo)).PortlandId;
            }
        }

        public async Task<IEnumerable<FgTransaction>> GetAllFGTransactionsThatNeedToBeInvoiced(DateOnly InvoiceDate)
        {
            return _db.FgTransactions.Where(e => e.Invoiced != true);
        }
        public void UpdatePortlandIdOnTransaction(GenericTransactionFile item, int? portlandId)
        {
            switch (item.network)
            {
                case 0:
                    var transaction1 = _db.KfE1E3Transactions.FirstOrDefault(e => e.TransactionId == item.TransactionId);
                    transaction1.PortlandId = portlandId;
                    _db.KfE1E3Transactions.Update(transaction1);
                    _db.SaveChanges();
                    return;
                case 1:
                    var transaction2 = _db.UkfTransactions.FirstOrDefault(e => e.TransactionId == item.TransactionId);
                    transaction2.PortlandId = portlandId;
                    _db.UkfTransactions.Update(transaction2);
                    _db.SaveChanges();
                    return;
                case 2:
                    var transaction3 = _db.TexacoTransactions.FirstOrDefault(e => e.TransactionId == item.TransactionId);
                    transaction3.PortlandId = portlandId;
                    _db.TexacoTransactions.Update(transaction3);
                    _db.SaveChanges();
                    return;
                case 3:
                    var transaction4 = _db.FgTransactions.FirstOrDefault(e => e.TransactionId == item.TransactionId);
                    transaction4.PortlandId = portlandId;
                    _db.FgTransactions.Update(transaction4);
                    _db.SaveChanges();
                    return;
            }
        }
        public List<List<GenericTransactionFile>> GroupTransactionsByCustomer(List<GenericTransactionFile> transactions)
        {

            List<List<GenericTransactionFile>> TransactionsByCustomer = transactions
       .GroupBy(t => t.CustomerCode)
       .Select(group => group.ToList())
       .ToList();
            var AllAquaid = TransactionsByCustomer
                .SelectMany(list => list)
                .Where(e => e.PortlandId == 100028 || e.PortlandId == 100029 || e.PortlandId == 100030 || e.PortlandId == 100031 || e.PortlandId == 100032 || e.PortlandId == 100494)
                .ToList();
            if (AllAquaid.Count > 0)
            {
                foreach (var CostCentre in AllAquaid)
                {
                    CostCentre.CustomerCode = GetAccountFromCostCentre(CostCentre.CardNumber);
                }
                var groupedAquaid = AllAquaid.GroupBy(e => e.CustomerCode).ToList();
                foreach (var aquaidList in AllAquaid)
                {
                    TransactionsByCustomer.RemoveAll(transactionsList => transactionsList.Contains(aquaidList));
                }
                foreach (var Transaction in groupedAquaid)
                {
                    TransactionsByCustomer.Add(Transaction.ToList());
                }
            }
            return TransactionsByCustomer;


        }

        private int? GetAccountFromCostCentre(decimal? cardNumber)
        {
            var AllCards = _db.FcHiddenCards.Where(e=>e.Id > -1);
            foreach (var item in AllCards)
            {
                if (cardNumber.ToString().Contains(item.CardNo))
                {
                    return item.AccountNo;
                }
            }
            throw new Exception($"Error on masked card {cardNumber}");
        }
        public EnumHelper.InvoiceFormatType? GetInvoiceFormatType(string networkName, int portlandId)
        {
            EnumHelper.Network networkEnum = EnumHelper.NetworkEnumFromString(networkName);
            int? DisplayGroup = _db.InvoicingOptions.FirstOrDefault(e=>e.GroupedNetwork.Contains((int)networkEnum) && e.PortlandId == portlandId)?.Displaygroup;
            if(DisplayGroup is null) return EnumHelper.InvoiceFormatType.Default;
            if (DisplayGroup == 1) return EnumHelper.InvoiceFormatType.Pan;
            else return null;
        }
    }
}
