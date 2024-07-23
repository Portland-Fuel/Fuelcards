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
using System.Transactions;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<CustomerInvoice>?> GetCustomersToInvoice(int network, DateOnly invoiceDate, double? BasePrice)
        {
            List<GenericTransactionFile> PortlandTransaction = new();
            List<List<GenericTransactionFile>> TransactionsByCustomerAndNetwork = new();
            List<CustomerInvoice> Customers = new();
            switch (network)
            {
                case 0:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(GetAllKeyfuelTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), null, null, null);
                    break;
                case 1:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(null, GetAllUKTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), null, null);
                    break;
                case 2:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(null, null, GetAllTexTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), null);
                    break;
                case 3:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(null, null, null, GetAllFGTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList());
                    break;
            }
            if (PortlandTransaction is null) return null;
            TransactionsByCustomerAndNetwork = GroupTransactionsByCustomer(PortlandTransaction);
            foreach (var item in TransactionsByCustomerAndNetwork)
            {
                CustomerInvoice model = new();
                model.name = HomeController.PFLXeroCustomersData.Where(e => e.ContactID.ToString() == GetXeroIdFromPortlandId(item[0].PortlandId)).FirstOrDefault()?.Name;
                if (model.name.ToLower().Contains("aquaid"))
                {
                    model.name = getAquidNameFromAccount(item[0].CustomerCode);
                    int? portlandID = GetAquaidPortlandIdFromName(model.name);
                    model.addon = (_db.CustomerPricingAddons.Where(e => e.PortlandId == portlandID && e.Network == (int)network && e.EffectiveDate <= item[0].TransactionDate).OrderByDescending(e => e.EffectiveDate).FirstOrDefault()?.Addon);
                }
                else
                {
                    model.addon = (_db.CustomerPricingAddons.Where(e => e.PortlandId == item[0].PortlandId && e.Network == (int)network && e.EffectiveDate <= item[0].TransactionDate).OrderByDescending(e => e.EffectiveDate).FirstOrDefault()?.Addon);
                }
                model.addon = BasePrice + model.addon;
                model.account = item[0].CustomerCode;
                Customers.Add(model);
            }
            return Customers;
        }

        private int? GetAquaidPortlandIdFromName(string? name)
        {
            int? portlandId = _db.FcHiddenCards.FirstOrDefault(e => e.CostCentre == name)?.PortlandId;
            if (portlandId is null) throw new ArgumentException($"Aquaid customers portland Id cannot be established from the cost centre name {name}.");
            return portlandId;
        }

        private string? getAquidNameFromAccount(int? customerAc)
        {
            string? name = _db.FcHiddenCards.FirstOrDefault(e => e.AccountNo == customerAc)?.CostCentre;
            if (name is null) throw new ArgumentException("Aquaid customer still cannot be established.");
            return name;
        }


        public string? GetXeroIdFromPortlandId(int? portlandId)
        {
            string? xero = _Cdb.PortlandIdToXeroIds.FirstOrDefault(e => e.PortlandId == portlandId && e.XeroTennant == 0)?.XeroId;
            return xero;
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
        public void UpdateAddon(NewCustomerDetailsModel.AddonData newAddon, string CustomerName, EnumHelper.Network network)
        {
            if (newAddon == null) return;
            try
            {
                int? PortlandId = GetPortlandIdFromCustomerName(CustomerName);
                if (PortlandId is null) throw new ArgumentException("Portland Id could not be established from the customer name");
                ProcessAddonList(newAddon, PortlandId, network);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public int? GetPortlandIdFromCustomerName(string customerName)
        {
            string xeroID = HomeController.PFLXeroCustomersData.Where(e => e.Name == customerName).FirstOrDefault().ContactID.ToString();
            int? portlandId = _Cdb.PortlandIdToXeroIds.FirstOrDefault(e => e.XeroId == xeroID && e.XeroTennant == 0).PortlandId;
            if (portlandId is null) throw new ArgumentException($"Portland ID could not be established from the xero id {xeroID}");
            return portlandId;
        }

        private void ProcessAddonList(NewCustomerDetailsModel.AddonData newAddon, int? portlandId, EnumHelper.Network network)
        {
            if (newAddon == null) return;
            CustomerPricingAddon model = new();

            model.EffectiveDate = DateOnly.Parse(newAddon.effectiveFrom);
            if (model.EffectiveDate == null)
                throw new ArgumentException("Problem mapping the effective date from the Json object to the CustomerPricingAddon model");

            model.Addon = Convert.ToDouble(newAddon.addon);
            if (model.Addon == null)
                throw new ArgumentException("Problem converting the addon from a string to a double");
            model.Network = (int)network;
            model.PortlandId = portlandId;

            _db.CustomerPricingAddons.Add(model);
            _db.SaveChanges();
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
                    List<int> DieselProductCodes = new List<int> { 1, 70 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && DieselProductCodes.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity) / 100;
                    return TotalQuantity;
                case EnumHelper.Products.Gasoil:
                    break;
                case EnumHelper.Products.ULSP:
                    List<int> UnleadedProductCodes = new List<int> { 2 };
                    TotalQuantity = _db.TexacoTransactions
                        .Where(e => e.Invoiced != true && UnleadedProductCodes.Contains((int)e.ProdNo))
                        .Sum(e => e.Quantity) / 100;
                    return TotalQuantity;

                case EnumHelper.Products.Lube:
                    break;
                case EnumHelper.Products.Adblue:
                    List<int> AdblueProductCodes = new List<int> { 8, 18 };
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

            var controlIds = await _db.FcControls.Where(e => e.CreationDate <= InvoiceDate && e.Invoiced != true)
                .OrderByDescending(e => e.ControlId)
                .Select(e => e.ControlId)
                .ToListAsync();

            var TransactionsToUse = await _db.KfE1E3Transactions.Where(e => e.Invoiced != true || controlIds.Contains(e.ControlId)).ToListAsync();
            var PotentialSundrySales = await _db.KfE4SundrySales.Where(e => e.Invoiced != true).ToListAsync();
            if (PotentialSundrySales?.Any() == true)
            {
                // Ensure transactionsToUse is a List (if it's not already)
                var editableTransactions = TransactionsToUse as List<KfE1E3Transaction> ?? TransactionsToUse.ToList();
                foreach (var item in PotentialSundrySales)
                {
                    editableTransactions.Add(await ConvertSundryToTransaction(item));
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
        public async Task<int>? GetPortlandIdFromAccount(int account)
        {
            try
            {
                var portlandId = await _db.FcNetworkAccNoToPortlandIds.FirstOrDefaultAsync(e => e.FcAccountNo == account);
                return portlandId.PortlandId;
            }
            catch (Exception chuckles)
            {

                throw;
            }

        }
        public async Task<KfE1E3Transaction> ConvertSundryToTransaction(KfE4SundrySale sundrysale)
        {

            KfE1E3Transaction model = new();
            model.AccurateMileage = null;
            model.CardNumber = sundrysale.CardNumber;
            model.CardRegistration = null;
            model.Commission = null;
            model.ControlId = (int)sundrysale.ControlId;
            model.CostSign = null;
            model.Cost = sundrysale.Value;
            model.CustomerAc = sundrysale.CustomerAc;
            model.CustomerCode = sundrysale.CustomerCode;
            model.FleetNumber = null;
            model.Invoiced = sundrysale.Invoiced;
            model.InvoiceNumber = null;
            model.InvoicePrice = null;
            model.Mileage = null;
            model.Period = sundrysale.Period;
            model.PortlandId = await GetPortlandIdFromAccount((int)sundrysale.CustomerCode);
            model.PrimaryRegistration = null;
            model.ProductCode = sundrysale.ProductCode;
            model.PumpNumber = null;
            model.Quantity = sundrysale.Quantity;
            model.ReportType = null;
            model.Sign = null;
            model.SiteCode = null;
            model.TransactionDate = sundrysale.TransactionDate;
            model.TransactionTime = sundrysale.TransactionTime;
            model.TransactionId = sundrysale.Id;
            model.TransactionNumber = sundrysale.TransactionNumber;
            model.TransactionSequence = sundrysale.TransactionSequence;
            model.TransactionType = sundrysale.TransactionType;
            model.TransactonRegistration = sundrysale.VehicleRegistration;

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
            var AllCards = _db.FcHiddenCards.Where(e => e.Id > -1);
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
            int? DisplayGroup = _db.InvoicingOptions.FirstOrDefault(e => e.GroupedNetwork.Contains((int)networkEnum) && e.PortlandId == portlandId)?.Displaygroup;
            if (DisplayGroup is null) return EnumHelper.InvoiceFormatType.Default;
            if (DisplayGroup == 1) return EnumHelper.InvoiceFormatType.Pan;
            else return null;
        }
        public List<int?>? CheckForDuplicateTransactions(EnumHelper.Network network)
        {
            List<int?> duplicateTransactions = new();
            switch (network)
            {
                case EnumHelper.Network.Keyfuels:

                    duplicateTransactions = _db.KfE1E3Transactions.AsEnumerable().Where(e => e.Invoiced != true).GroupBy(e => e.TransactionNumber).Where(g => g.Count() > 1).SelectMany(g => g).Select(e => e.TransactionNumber).ToList();
                    return duplicateTransactions;
                case EnumHelper.Network.UkFuel:
                    duplicateTransactions = _db.UkfTransactions.AsEnumerable().Where(e => e.Invoiced != true).GroupBy(e => e.TranNoItem).Where(g => g.Count() > 1).SelectMany(g => g).Select(e => e.TranNoItem).ToList();
                    return duplicateTransactions;
                case EnumHelper.Network.Texaco:
                    duplicateTransactions = _db.TexacoTransactions.AsEnumerable().Where(e => e.Invoiced != true).GroupBy(e => e.TranNoItem).Where(g => g.Count() > 1).SelectMany(g => g).Select(e => e.TranNoItem).ToList();
                    return duplicateTransactions;
                default: return null;
            }
        }
        public async void UpdateAccount(NewCustomerDetailsModel.AccountInfo updatedAccount, string CustomerName, EnumHelper.Network network)
        {
            FcEmail? ExistingEmail = _db.FcEmails.FirstOrDefault(e => e.Account == Convert.ToInt32(updatedAccount.account));
            ExistingEmail.To = updatedAccount.toEmail;
            ExistingEmail.Cc = updatedAccount.ccEmail;
            ExistingEmail.Bcc = updatedAccount.BccEmail;
            await FcEmailUpdateAsync(ExistingEmail);
            _db.SaveChanges();
        }
        public async Task FcEmailUpdateAsync(FcEmail source)
        {
            var dbObj = _db.FcEmails.FirstOrDefault(s => s.Account == source.Account);
            if (dbObj is null) await _db.FcEmails.AddAsync(source);
            else FcEmailUpdateDbObject(dbObj, source);
        }
        public void FcEmailUpdateDbObject(FcEmail dbObj, FcEmail source)
        {
            dbObj.Account = dbObj.Account;
            dbObj.To = source.To;
            dbObj.Cc = source.Cc;
            dbObj.Bcc = source.Bcc;
        }
        public async Task NewFix(NewCustomerDetailsModel.Fix item, string customerName, EnumHelper.Network network)
        {
            FixedPriceContract model = new();
            model.TradeReference = Convert.ToDecimal(item.tradeReference);
            model.EffectiveFrom = DateOnly.Parse(item.effectiveFrom);
            model.EndDate = DateOnly.Parse(item.endDate);
            model.FixedPrice = Convert.ToDouble(item.fixedPrice);
            model.FixedVolume = Convert.ToInt32(item.fixedVolume);
            model.FixedPriceIncDuty = Convert.ToDouble(item.fixedPriceIncDuty);
            model.Grade = GetGradeIdFromGradeString(item.grade);
            model.FcAccount = Convert.ToInt32(item.account);
            model.FrequencyId = GetFrequencyIdFromStringPeriod(item.period);
            model.Network = GetNetworkArrayFromAccountNumber(Convert.ToInt32(item.account)).ToList();
            model.PortlandId = await GetPortlandIdFromAccount(Convert.ToInt32(item.account));
            await FixedPriceContractUpdateAsync(model);
            _db.SaveChanges();
        }

        private int[]? GetNetworkArrayFromAccountNumber(int? account)
        {
            int[] Returner = new int[1];
            int? network = Convert.ToInt32(_db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e => e.FcAccountNo == account)?.Network);
            if (network.HasValue)
            {
                Returner[0] = network.Value;
            }
            return Returner;
        }

        private int? GetGradeIdFromGradeString(string? grade)
        {
            int? id = _db.FcGrades.FirstOrDefault(e => e.Grade == grade)?.Id;
            if (id is null) throw new ArgumentException($"Grade Id could not be established from the grade string {grade}");
            return id;
        }

        private int? GetFrequencyIdFromStringPeriod(string? period)
        {
            int? id = _db.FixFrequencies.FirstOrDefault(e => e.FrequencyPeriod == period)?.FrequencyId;
            if (id is null) throw new ArgumentException($"Frequency ID could not be established from the frequency period of {period}");
            return id;
        }

        public async Task FixedPriceContractUpdateAsync(FixedPriceContract source)
        {
            var dbObj = _db.FixedPriceContracts.FirstOrDefault(s => s.TradeReference == source.TradeReference);
            if (dbObj is null) await _db.FixedPriceContracts.AddAsync(source);
            else FixedPriceContractUpdateDbObject(dbObj, source);
        }
        public void FixedPriceContractUpdateDbObject(FixedPriceContract dbObj, FixedPriceContract source)
        {
            dbObj.TradeReference = source.TradeReference;
            dbObj.EffectiveFrom = source.EffectiveFrom;
            dbObj.EndDate = source.EndDate;
            dbObj.FixedPrice = source.FixedPrice;
            dbObj.FixedPriceIncDuty = source.FixedPriceIncDuty;
            dbObj.Grade = source.Grade;
            dbObj.TerminationDate = source.TerminationDate;
            dbObj.FcAccount = source.FcAccount;
        }

    }
}
