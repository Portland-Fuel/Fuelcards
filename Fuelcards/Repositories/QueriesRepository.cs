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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DataAccess.Tickets;
using Site = Fuelcards.Models.Site;
using static Fuelcards.GenericClassFiles.EnumHelper;
namespace Fuelcards.Repositories
{
    public class QueriesRepository : IQueriesRepository
    {
        private readonly FuelcardsContext _db;
        private readonly CDataContext _Cdb;
        private readonly IfuelsContext _Idb;
        public QueriesRepository(FuelcardsContext db, CDataContext cDb, IfuelsContext Idb)
        {
            _db = db;
            _Cdb = cDb;
            _Idb = Idb;
        }
        public void UploadNewItemInventoryCode(string description, string itemCode)
        {
            _db.ProductDescriptionToInventoryItemCodes.Add(new ProductDescriptionToInventoryItemCode { Description = description.Trim(), InventoryItemcode = itemCode.Trim() });
            _db.SaveChanges();
        }
        public List<Dictionary<string, string>> GetListOfProducts()
        {
            List<Dictionary<string, string>> toreturn = new();
            var ff = _db.ProductDescriptionToInventoryItemCodes
                 .Where(e => e.Id > 0)
                 .ToList();

            foreach (var item in ff)
            {
                Dictionary<string, string> dict = new();
                dict.Add(item.Description, item.InventoryItemcode);

                toreturn.Add(dict);
            }

            return toreturn;
        }
        public string? GetinventoryItemCode(string productName)
        {
            string? ItemInventoryCode = _db.ProductDescriptionToInventoryItemCodes.FirstOrDefault(e => e.Description.ToLower() == productName.ToLower())?.InventoryItemcode;
            if (ItemInventoryCode is null) return null; else { return ItemInventoryCode; }
        }
        public void AddSiteNumberToBand(Site site)
        {
            SiteNumberToBand siteNumberToBand = new()
            {
                SiteNumber = site.code,
                NetworkId = (int)EnumHelper.NetworkEnumFromString(site.Network),
                Band = site.band,
                Active = true,
                Name = site.name,
                Surcharge = site.Surcharge,
                EffectiveDate = GetEffectiveDate(),
            };
            _db.SiteNumberToBands.Add(siteNumberToBand);
            _db.SaveChanges();
        }
        private DateOnly GetEffectiveDate()
        {
            return DateOnly.FromDateTime(DateTime.Now).AddDays(-40);
        }
        public bool CheckSite(Site item)
        {
            return _db.SiteNumberToBands.Any(e => e.SiteNumber == item.code && e.NetworkId == (int)EnumHelper.NetworkEnumFromString(item.Network) && e.Active != false);
        }
        public List<Models.Site> GetAllTransactions(List<int> ControlIDs)
        {
            try
            {
                List<Models.Site> AllSites = new();
                var KeyfuelTransactions = _db.KfE1E3Transactions.Where(e => ControlIDs.Contains(e.ControlId));
                var UKfuelTransactions = _db.UkfTransactions.Where(e => ControlIDs.Contains(e.ControlId));
                var TexacoTransactions = _db.TexacoTransactions.Where(e => ControlIDs.Contains((int)e.ControlId));

                foreach (var item in KeyfuelTransactions)
                {
                    Models.Site newSite = new Models.Site
                    {
                        code = item.SiteCode,
                        Network = "KeyFuels"
                    };

                    AllSites.Add(newSite);
                }

                foreach (var item in UKfuelTransactions)
                {
                    Models.Site newSite = new Models.Site
                    {
                        code = Convert.ToInt32(item.Site),
                        Network = "UkFuels",
                    };

                    AllSites.Add(newSite);
                }
                foreach (var item in TexacoTransactions)
                {
                    Models.Site newSite = new Models.Site
                    {
                        code = Convert.ToInt32(item.Site),
                        Network = "Texaco",
                    };

                    AllSites.Add(newSite);
                }

                return AllSites;


            }
            catch (Exception e)
            {
                throw new Exception("Error Getting All Transactions:" + e.Message);
            }
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
            if (xeroId == "FTC") return 7;
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
            var fuelcardBasePrice = _db.FuelcardBasePrices
                                             .FirstOrDefault(e => e.EffectiveFrom <= invoiceDate && e.EffectiveTo >= invoiceDate);
            return fuelcardBasePrice?.BasePrice;
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
            var sites = GetAllSiteInformation();
            List<List<GenericTransactionFile>> TransactionsByCustomerAndNetwork = new();
            List<CustomerInvoice> Customers = new();
            switch (network)
            {
                case 0:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(GetAllKeyfuelTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), null, null, null, sites);
                    break;
                case 1:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(null, GetAllUKTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), null, null, sites);
                    break;
                case 2:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(null, null, GetAllTexTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), null, sites);
                    break;
                case 3:
                    PortlandTransaction = Transactions.TurnTransactionIntoGeneric(null, null, null, GetAllFGTransactionsThatNeedToBeInvoiced(invoiceDate).Result.ToList(), sites);
                    break;
            }
            List<FcHiddenCard> aquaid = await GetAquaidInfo();
            List<PortlandIdToXeroId>? PortlandIdsToXeroIds = await GetXeroIdFromPortlandId();
            List<CustomerPricingAddon>? customerAddons = await GetCustomerAddons(EnumHelper.NetworkEnumFromInt(network));

            if (PortlandTransaction is null) return null;
            TransactionsByCustomerAndNetwork = GroupTransactionsByCustomer(PortlandTransaction);
            try
            {


                foreach (var item in TransactionsByCustomerAndNetwork)
                {
                    CustomerInvoice model = new();
                    model.name = HomeController.PFLXeroCustomersData.Where(e => e.ContactID.ToString() == PortlandIdsToXeroIds.Where(e => e.PortlandId == item[0].portlandId && e.XeroTennant == 0).FirstOrDefault()?.XeroId).FirstOrDefault()?.Name;
                    if (model.name is null) throw new ArgumentException("could not get Name from portland ID. Possibly the customer is missing in the portland_id to xero_id table");
                    if (model.name.ToLower().Contains("aquaid"))
                    {
                        model.name = aquaid.FirstOrDefault(e => e.AccountNo == item[0].customerCode)?.CostCentre;

                        int? portlandID = aquaid.FirstOrDefault(e => e.CostCentre == model.name)?.PortlandId;
                        model.addon = (customerAddons.Where(e => e.PortlandId == portlandID && e.Network == (int)network && e.EffectiveDate <= item[0].transactionDate).OrderByDescending(e => e.EffectiveDate).FirstOrDefault()?.Addon);
                    }
                    else
                    {
                        model.addon = (customerAddons.Where(e => e.PortlandId == item[0].portlandId && e.Network == (int)network && e.EffectiveDate <= item[0].transactionDate).OrderByDescending(e => e.EffectiveDate).FirstOrDefault()?.Addon);
                    }
                    if (model.name.ToLower().Contains("portland")) model.name = "The Fuel Trading Company";
                    model.addon = Convert.ToDouble(Math.Round(Convert.ToDecimal(BasePrice + model.addon), 2));
                    if (model.addon == 0) throw new ArgumentException($"No Addon found for the following customer - {model.name} with account {model.account}");
                    model.account = item[0].customerCode;
                    model.CustomerTransactions = new();
                    model.CustomerType = await customerType((int)model.account, invoiceDate);
                    model.invoiceDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now.AddDays(-7)));
                    var portlandId = GetPortlandIdFromAccount((int)model.account).Result;
                    var invoiceType = _db.InvoicingOptions.FirstOrDefault(e => e.PortlandId == portlandId && e.GroupedNetwork.Contains((int)item[0].network))?.Displaygroup;
                    if (invoiceType == 1)
                    {
                        model.CustomerTransactions = item.OrderBy(e => e.cardNumber).ThenBy(e => e.transactionDate).ThenBy(e => e.transactionTime).ToList();
                    }
                    else
                    {
                        model.CustomerTransactions = item.OrderBy(e => e.transactionDate).ThenBy(e => e.transactionTime).ToList();
                    }
                    model.IfuelsCustomer = IfuelsCustomer((int)model.account);
                    if (model.CustomerType != EnumHelper.CustomerType.Floating)
                    {
                        model.fixedInformation = new FixedInformation();
                        model = FixedProperties(model, invoiceDate, model.CustomerType);
                    }
                    Customers.Add(model);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
           var CustomersOrdered = Customers.OrderBy(e => e.name).ToList();

            return CustomersOrdered;
        }
        private CustomerInvoice? FixedProperties(CustomerInvoice model, DateOnly invoiceDate, EnumHelper.CustomerType custType)
        {
            try
            {
                List<FixedPriceContract> fixedContracts = _db.FixedPriceContracts.Where(e => e.FcAccount == model.account).ToList();
                if (custType == EnumHelper.CustomerType.Fix)
                {
                    model.fixedInformation.AllFixes = ConvertFixedPriceToVM(fixedContracts);
                    var tradeIds = model.fixedInformation.AllFixes
                        .Select(f => f.Id)
                        .ToList();
                    model.fixedInformation.CurrentAllocation = GetCurrentAllocation(invoiceDate, tradeIds);
                    model.fixedInformation.CurrentTradeId = GetTradeIdFromAllocationId(model.fixedInformation.CurrentAllocation);
                    model.fixedInformation.RolledVolume = _db.AllocatedVolumes
                        .Where(e => tradeIds.Contains((int)e.TradeId) && e.Volume > 0 && e.AllocationId < model.fixedInformation.CurrentAllocation)
                        .Sum(e => (double?)e.Volume) ?? 0;
                    return model;
                }
                else
                {
                    List<int> fixedContractIds = fixedContracts.Select(fc => fc.Id).ToList();
                    var alreadyAllocated = _db.FixAllocationDates
                        .Where(e => fixedContractIds.Contains((int)e.TradeId) && e.NewAllocationDate < invoiceDate)
                        .Select(f => f.Id)
                        .ToList();
                    var allocatedVolumeSum = _db.AllocatedVolumes
                        .Where(av => av.Volume > 0 && alreadyAllocated.Contains((int)av.AllocationId))
                        .Sum(x => x.Volume);

                    // Step 5: Assign the sum to model.fixedInformation.RolledVolume
                    model.fixedInformation.RolledVolume = InvoiceSummary.Round2(allocatedVolumeSum);

                    model.fixedInformation.CurrentTradeId = null;
                    return model;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Error getting fixed properties");
            }
        }

        private List<FixedPriceContractVM>? ConvertFixedPriceToVM(List<FixedPriceContract> fixedContracts)
        {
            List<FixedPriceContractVM> vmList = new();
            foreach (var item in fixedContracts)
            {
                FixedPriceContractVM model = new()
                {
                    TradeReference = item.TradeReference,
                    EffectiveFrom = item.EffectiveFrom,
                    EndDate = item.EndDate,
                    FcAccount = item.FcAccount,
                    FixedPrice = item.FixedPrice,
                    FixedVolume = item.FixedVolume,
                    FixedPriceIncDuty = item.FixedPriceIncDuty,
                    Network = item.Network,
                    FrequencyId = item.FrequencyId,
                    Id = item.Id,
                    Grade = item.Grade,
                    TerminationDate = item.TerminationDate,
                    Period = item.Period,
                    PortlandId = item.PortlandId,
                };
                vmList.Add(model);
            }
            return vmList;
        }

        private int? GetTradeIdFromAllocationId(int currentAllocation)
        {
            return _db.AllocatedVolumes.FirstOrDefault(e => e.AllocationId == currentAllocation)?.TradeId;
        }

        public int GetCurrentAllocation(DateOnly InvoiceDate, List<int>? tradeIds)
        {
            DateOnly CurrentDate = InvoiceDate.AddDays(-3);

            var id = _db.FixAllocationDates
                .Where(e => e.NewAllocationDate <= CurrentDate && CurrentDate <= e.AllocationEnd && tradeIds.Contains((int)e.TradeId))
                .Select(e => e.Id)
                .FirstOrDefault();
            return id;
        }



        private int? GetAquaidPortlandIdFromName(string? name)
        {
            int? portlandId = _db.FcHiddenCards.FirstOrDefault(e => e.CostCentre == name)?.PortlandId;
            if (portlandId is null) throw new ArgumentException($"Aquaid customers portland Id cannot be established from the cost centre name {name}.");
            return portlandId;
        }
        public async Task<List<FcHiddenCard>> GetAquaidInfo()
        {
            return await _db.FcHiddenCards.Where(e => e.Id > -1).ToListAsync();
        }
        public async Task<List<CustomerPricingAddon>> GetCustomerAddons(EnumHelper.Network network)
        {
            return await _db.CustomerPricingAddons.Where(e => e.Network == (int)network).ToListAsync();
        }

        public async Task<List<PortlandIdToXeroId>?> GetXeroIdFromPortlandId()
        {
            return await _Cdb.PortlandIdToXeroIds.Where(e => e.Id > -1).ToListAsync();
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
            if (customerName == "The Fuel Trading Company") return 100177;
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
        public async Task<List<int>> GetFailedSiteBanding(int network)
        {
            List<int> Failed = new();
            List<int?> Sites = new();
            List<int> fcControls = await _db.FcControls.Where(e => e.Invoiced != true).Select(e => e.ControlId).ToListAsync();
            switch (network)
            {
                case 0:
                    Sites = await _db.KfE1E3Transactions.Where(e => fcControls.Contains(e.ControlId)).Select(e => e.SiteCode).Distinct().ToListAsync();
                    break;
                case 1:
                    Sites = await _db.UkfTransactions.Where(e => fcControls.Contains(e.ControlId)).Select(e => (int?)e.Site).Distinct().ToListAsync();
                    break;
                case 2:
                    Sites = await _db.TexacoTransactions.Where(e => fcControls.Contains((int)e.ControlId)).Select(e => (int?)e.Site).Distinct().ToListAsync();
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
            var sites = GetAllSiteInformation();
            do
            {
                updatesMade = false;

                AllTransactions = Transactions.TurnTransactionIntoGeneric(
                    GetAllKeyfuelTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(),
                    GetAllUKTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(),
                    GetAllTexTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(),
                    GetAllFGTransactionsThatNeedToBeInvoiced(InvoiceDate).Result.ToList(), sites);

                foreach (var item in AllTransactions.Where(e => e.portlandId is null))
                {
                    try

                    {
                        var PortlandId = GetPortlandIdFromMaskedCards(Convert.ToDecimal(item.cardNumber));
                        if (PortlandId is not null)
                        {
                            UpdatePortlandIdOnTransaction(item, PortlandId);
                            updatesMade = true;
                        }
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"error with a masked card. The card number {item.cardNumber} Failed.");
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
                    var transaction1 = _db.KfE1E3Transactions.FirstOrDefault(e => e.TransactionId == item.transactionId);
                    transaction1.PortlandId = portlandId;
                    _db.KfE1E3Transactions.Update(transaction1);
                    _db.SaveChanges();
                    return;
                case 1:
                    var transaction2 = _db.UkfTransactions.FirstOrDefault(e => e.TransactionId == item.transactionId);
                    transaction2.PortlandId = portlandId;
                    _db.UkfTransactions.Update(transaction2);
                    _db.SaveChanges();
                    return;
                case 2:
                    var transaction3 = _db.TexacoTransactions.FirstOrDefault(e => e.TransactionId == item.transactionId);
                    transaction3.PortlandId = portlandId;
                    _db.TexacoTransactions.Update(transaction3);
                    _db.SaveChanges();
                    return;
                case 3:
                    var transaction4 = _db.FgTransactions.FirstOrDefault(e => e.TransactionId == item.transactionId);
                    transaction4.PortlandId = portlandId;
                    _db.FgTransactions.Update(transaction4);
                    _db.SaveChanges();
                    return;
            }
        }
        public List<List<GenericTransactionFile>> GroupTransactionsByCustomer(List<GenericTransactionFile> transactions)
        {

            List<List<GenericTransactionFile>> TransactionsByCustomer = transactions
       .GroupBy(t => t.customerCode)
       .Select(group => group.ToList())
       .ToList();
            var AllAquaid = TransactionsByCustomer
                .SelectMany(list => list)
                .Where(e => e.portlandId == 100028 || e.portlandId == 100029 || e.portlandId == 100030 || e.portlandId == 100031 || e.portlandId == 100032 || e.portlandId == 100494)
                .ToList();
            if (AllAquaid.Count > 0)
            {
                var AllFcHiddenCardData = GetAccountFromCostCentre();
                foreach (var CostCentre in AllAquaid)
                {
                    CostCentre.customerCode = AllFcHiddenCardData.Where(e => CostCentre.cardNumber.ToString().Contains(e.CardNo)).FirstOrDefault()?.AccountNo;
                }
                var groupedAquaid = AllAquaid.GroupBy(e => e.customerCode).ToList();
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

        private IQueryable<FcHiddenCard>? GetAccountFromCostCentre()
        {
            var AllCards = _db.FcHiddenCards.Where(e => e.Id > -1);
            return AllCards;
            //foreach (var item in AllCards)
            //{
            //    if (cardNumber.ToString().Contains(item.CardNo))
            //    {
            //        return item.AccountNo;
            //    }
            //}
            //throw new Exception($"Error on masked card {cardNumber}");
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
            var portlandId = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(e => e.FcAccountNo == Convert.ToInt32(updatedAccount.account)).PortlandId;
            var XeroIds = _Cdb.PortlandIdToXeroIds.FirstOrDefault(e => e.PortlandId == portlandId && e.XeroTennant == 0).XeroId;
            DataAccess.Fuelcards.PaymentTerm? ExistingTerms = _db.PaymentTerms.FirstOrDefault(e => e.XeroId == XeroIds && e.Network == (int)network);
            if (ExistingTerms is null)
            {
                DataAccess.Fuelcards.PaymentTerm newTerms = new();
                newTerms.Network = (int)network;
                newTerms.XeroId = XeroIds;
                newTerms.PaymentTerms = Convert.ToInt32(updatedAccount.paymentTerm);
                _db.PaymentTerms.Add(newTerms);
                _db.SaveChanges();
            }
            else
            {
                ExistingTerms.PaymentTerms = Convert.ToInt32(updatedAccount.paymentTerm);
                _db.PaymentTerms.Update(ExistingTerms);
                _db.SaveChanges();
            }
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
        public List<SiteNumberToBand> GetAllSiteInformation()
        {
            return _db.SiteNumberToBands.Where(e => e.Active != false).ToList();
        }

        public async Task<EnumHelper.CustomerType> customerType(int account, DateOnly invoiceDate)
        {
            bool HasUnusedVolume = false;
            IEnumerable<FixedPriceContract>? contracts = await _db.FixedPriceContracts.Where(e => e.FcAccount == Convert.ToInt32(account) && e.EffectiveFrom < invoiceDate).ToListAsync();
            if (contracts == null || !contracts.Any())
            {
                return EnumHelper.CustomerType.Floating;
            }
            foreach (var contract in contracts)
            {
                HasUnusedVolume = await CheckForUnusedVolumeOnThisContract(contract);
                if (contract.FrequencyId == 3)
                {
                    contract.EndDate = contract.EndDate.Value.AddMonths(1);
                }
                if (contract.FrequencyId == 1)
                {
                    contract.EndDate = contract.EndDate.Value.AddDays(7);
                }
                if (contract.EndDate >= invoiceDate)
                {
                    return EnumHelper.CustomerType.Fix;
                }
                else if (HasUnusedVolume)
                {
                    return EnumHelper.CustomerType.ExpiredFixWithVolume;
                }
            }
            return EnumHelper.CustomerType.Floating;
        }


        private async Task<bool> CheckForUnusedVolumeOnThisContract(FixedPriceContract contract)
        {
            var volume = _db.AllocatedVolumes.FirstOrDefaultAsync(e => e.Volume > 0 && e.TradeId == contract.Id).Result;
            if (volume == null) return false;
            return true;
        }

        public double? GetSurchargeFromBand(string? band, EnumHelper.Network network)
        {
            // Convert the band if it is a letter
            band = ConvertBandToNumber(band);

            double? surcharge = _db.SiteBandInfos.FirstOrDefault(e => e.NetworkId == (int)network && e.Band == band)?.CommercialPrice;
            if (surcharge == null) throw new ArgumentException("Surcharge should not be null, even if it is 0");
            return surcharge;
        }
        private string ConvertBandToNumber(string? band)
        {
            if (string.IsNullOrEmpty(band))
            {
                throw new ArgumentException("Band should not be null or empty");
            }

            // Check if the band is an alphabetical letter
            if (char.IsLetter(band[0]))
            {
                char upperBand = char.ToUpper(band[0]);
                int bandNumber = upperBand - 'A' + 1;
                return bandNumber.ToString();
            }

            // If band is already numeric, return it unchanged
            return band;
        }

        public double? GetAddonForSpecificTransaction(int? portlandId, DateOnly? transactionDate, EnumHelper.Network network, bool isIfuels, int account)
        {
            if (!isIfuels)
            {
                portlandId = GetPortlandIdFromAquaidAccount(portlandId, account);
                if (portlandId is null) throw new ArgumentException("Portland ID should not be null at this stage.");
                return _db.CustomerPricingAddons.Where(e => e.PortlandId == portlandId && e.Network == (int)network && e.EffectiveDate <= transactionDate).OrderByDescending(e => e.EffectiveDate).FirstOrDefault()?.Addon;
            }
            else
            {
                return GetIfuelsAddon(account, network);
            }
            return 0;
        }

        private int? GetPortlandIdFromAquaidAccount(int? portlandId, int account)
        {
            switch (account)
            {
                case 139461:return 100028;
                case 677112:return 100030;
                case 139464:return 100032;
                case 139462:return 100029;
                case 139463:return 100031;
                default: return portlandId;
            }
        }

        public bool IfuelsCustomer(int account)
        {
            bool Exists = _Idb.IfuelsCustomers.FirstOrDefault(e => e.CustomerNumber == account) != null;
            return Exists;
        }
        internal double? GetIfuelsAddon(int account, EnumHelper.Network network)
        {
            var addon = _Idb.IfuelsAddons
                .Where(e => e.CustomerNumber == account)
                .OrderByDescending(e => e.EffectiveFrom)
                .Select(e => e.Addon)
                .FirstOrDefault();
            double? ifuelsCost = _db.TransactionSiteSurcharges.FirstOrDefault(e => e.Network == (int)network && e.ChargeType == "i-fuelcards cost price")?.Surcharge;
            return addon + ifuelsCost;
        }
        public double? TransactionalSiteSurcharge(EnumHelper.Network network, int site, int productCode)
        {
            if (network == EnumHelper.Network.Keyfuels || network == EnumHelper.Network.Fuelgenie) return 0;
            TransactionalSite? result = _db.TransactionalSites.FirstOrDefault(e => e.SiteCode == site && e.Network == (int)network);
            if (result != null || productCode == 70)
            {
                double? Surcharge = _db.TransactionSiteSurcharges.FirstOrDefault(e => e.Network == (int)network && e.ChargeType == null)?.Surcharge;
                return Surcharge;
            }
            return 0;
        }
        public double? GetMissingProduct(EnumHelper.Network network, short? productCode)
        {
            return _db.MissingProductValues.FirstOrDefault(e => e.Network == (int)network && e.Product == productCode)?.Value;
        }
        public int GetInvoiceDisplayGroup(string companyName, string network)
        {
            var PortlandId = GetPortlandIdFromCustomerName(companyName);
            int? DisplayGroup = _db.InvoicingOptions.FirstOrDefault(e => e.PortlandId == PortlandId && e.GroupedNetwork.Contains((int)EnumHelper.NetworkEnumFromString(network)))?.Displaygroup;
            if (DisplayGroup == null) return 0;
            else return (int)DisplayGroup;
        }
        public double? GetRemaingVolumeForCurrentAllocation(int currentAllocation)
        {
            return _db.AllocatedVolumes.FirstOrDefault(e => e.AllocationId == currentAllocation)?.Volume;
        }
        public string? getNewInvoiceNumber(int network)
        {
            //string prefix = string.Empty;
            //switch (network)
            //{
            //    case 1:
            //        prefix = "PF";
            //        break;
            //    case 0:
            //        prefix = "";
            //        break;
            //    case 3:
            //        prefix = "TX";
            //        break;
            //    default:
            //        break;

            //}
            InvoiceNumber model = new()
            {
                Network = network,
            };
            _db.InvoiceNumbers.Add(model);
            _db.SaveChanges();
            return model.InvoiceNumber1.ToString();


        }
        public double? GetHandlingCharge(int network)
        {
            return _db.TransactionSiteSurcharges.FirstOrDefault(e => e.Network == (int)network && e.ChargeType == "Texaco Handling Charge")?.Surcharge;
        }
       public async Task ConfirmChanges(string network, List<InvoiceReport> reports, List<InvoicePDFModel> invoices)
        {
            EnumHelper.Network NetworkEnum = EnumHelper.NetworkEnumFromString(network);

            var Transactions = await GetCustomersToInvoice((int)NetworkEnum, invoices[0].InvoiceDate, 0);
            //THIS IS NOT DONE
        }
    }
}