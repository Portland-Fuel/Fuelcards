using Fuelcards.Models;
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Controllers;

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
        List<CustomerInvoice>? GetCustomersToInvoice(int network, DateOnly invoiceDate, double? BasePrice);
        List<int>? GetAllFixedCustomers(DateOnly InvoiceDate, int network);
        //void UpdateAddon(CustomerDetailsModels.AddonFromJs? customerPricingAddon);
        List<int> GetFailedSiteBanding(int network);
        double? GetProductVolume(EnumHelper.Products product);
        int? GetPortlandIdFromAccount(int account);
        Task<IEnumerable<FgTransaction>> GetAllFGTransactionsThatNeedToBeInvoiced(DateOnly InvoiceDate);
        void UpdatePortlandIdOnTransaction(GenericTransactionFile item, int? portlandId);
        List<List<GenericTransactionFile>> GroupTransactionsByCustomer(List<GenericTransactionFile> transactions);
        EnumHelper.InvoiceFormatType? GetInvoiceFormatType(string networkName, int portlandId);
        string? GetXeroIdFromPortlandId(int? portlandId);
        void UpdateAddon(NewCustomerDetailsModel.AddonData newAddon, List<NewCustomerDetailsModel.AccountInfo>? newAccountInfo);
    }
}