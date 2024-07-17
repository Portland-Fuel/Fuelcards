using Fuelcards.Models;
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;

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
        List<CustomerInvoice>? GetCustomersToInvoice(int network, DateOnly invoiceDate);
        List<int>? GetAllFixedCustomers(DateOnly InvoiceDate, int network);

    }
}