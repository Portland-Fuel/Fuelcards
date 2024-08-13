using Fuelcards.Models;
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Controllers;
using DataAccess.Cdata;
using Fuelcards.InvoiceMethods;

namespace Fuelcards.Repositories
{
    public interface IQueriesRepository
    {
        List<CustomerPricingAddon>? GetListOfAddonsForCustomer(int portlandId, EnumHelper.Network network);
        int? GetPortlandIdFromXeroId(string xeroId);
        int? GetPaymentTerms(string xeroId);
        List<FixedPriceContract>? AllFixContracts(int account);
        Email AllEmail(int account);
        int[]? GetAccounts(int portlandId);
        EnumHelper.Network getNetworkFromAccount(int account);
        double? GetBasePrice(DateOnly invoiceDate);
        int? GetTotalEDIs(int network);
        double? GetDieselBand7Texaco();
        Task<List<CustomerInvoice>?> GetCustomersToInvoice(int network, DateOnly invoiceDate, double? BasePrice);
        List<int>? GetAllFixedCustomers(DateOnly InvoiceDate, int network);
        //void UpdateAddon(CustomerDetailsModels.AddonFromJs? customerPricingAddon);
        Task<List<int>> GetFailedSiteBanding(int network);
        double? GetProductVolume(EnumHelper.Products product);
        Task<int>? GetPortlandIdFromAccount(int account);
        Task<IEnumerable<FgTransaction>> GetAllFGTransactionsThatNeedToBeInvoiced(DateOnly InvoiceDate);
        void UpdatePortlandIdOnTransaction(GenericTransactionFile item, int? portlandId);
        List<List<GenericTransactionFile>> GroupTransactionsByCustomer(List<GenericTransactionFile> transactions);
        EnumHelper.InvoiceFormatType? GetInvoiceFormatType(string networkName, int portlandId);
        Task<List<PortlandIdToXeroId>?> GetXeroIdFromPortlandId();
        void UpdateAddon(NewCustomerDetailsModel.AddonData newAddon, string CustomerName, EnumHelper.Network network);
        List<int?>? CheckForDuplicateTransactions(EnumHelper.Network network);
        void UpdateAccount(NewCustomerDetailsModel.AccountInfo updatedAccount, string CustomerName, EnumHelper.Network network);

        Task FcEmailUpdateAsync(FcEmail source);
        Task NewFix(NewCustomerDetailsModel.Fix newFixesForCustomer, string customerName, EnumHelper.Network network);
        List<SiteNumberToBand> GetAllSiteInformation();
        Task<EnumHelper.CustomerType> customerType(int account, DateOnly invoiceDate);
        double? GetSurchargeFromBand(string? band, EnumHelper.Network network);
        double? GetAddonForSpecificTransaction(int? portlandId, DateOnly? transactionDate, EnumHelper.Network network, bool isIfuels, int account);
        double? TransactionalSiteSurcharge(EnumHelper.Network network, int site, int productCode);
        double? GetMissingProduct(EnumHelper.Network network, short? productCode);
        int GetInvoiceDisplayGroup(string companyName, string network);
        List<Models.Site> GetAllTransactions(List<int> ControlIDs);
        bool CheckSite(Site item);
        void AddSiteNumberToBand(Site site);
        string? GetinventoryItemCode(string productName);

        double? GetRemaingVolumeForCurrentAllocation(int currentAllocation);
        void UploadNewItemInventoryCode(string description, string itemCode);
        List<Dictionary<string, string>> GetListOfProducts();
        string? getNewInvoiceNumber(int network);
        double? GetHandlingCharge(int network);
        void ConfirmChanges(string network, List<InvoiceReport> reports, List<InvoicePDFModel> invoices);

    }
}