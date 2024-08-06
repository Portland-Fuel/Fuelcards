
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;

namespace Fuelcards.Models
{
    public class InvoicePreCheckModels
    {
        public DateOnly InvoiceDate { get; set; }
        public double? BasePrice { get; set; }
        public double? PlattsPrice { get; set; }
        public int? KeyfuelImports { get; set; }
        public int? UkfuelImports { get; set; }
        public int? TexacoImports { get; set; }
        public List<CustomerInvoice> KeyfuelsInvoiceList { get; set; }
        public List<CustomerInvoice> UkFuelInvoiceList { get; set; }
        public List<CustomerInvoice>? TexacoInvoiceList { get; set; }
        public List<CustomerInvoice>? FuelgenieInvoiceList { get; set; }
        public List<int> FailedKeyfuelsSites { get; set; }
        public List<int> FailedUkfuelSites { get; set; }
        public List<int> FailedTexacoSites { get; set; }
        public TexacoVolumes TexacoVolume { get; set; }
        public List<int?>? KeyfuelsDuplicates { get; set; }
        public List<int?>? UkFuelDuplicates { get; set; }
        public List<int?>? TexacoDuplicates { get; set; }
    }
    public class CustomerInvoice
    {
        public string name { get; set; }
        public double? addon { get; set; }
        public int? account { get; set; }
        public EnumHelper.CustomerType CustomerType { get; set; }
        public bool IfuelsCustomer { get; set; }
        public double? BasePrice { get; set; }
        public FixedInformation? fixedInformation { get; set; }
        public List<GenericTransactionFile>? CustomerTransactions { get; set; }
    }


    public class FixedInformation()
    {
        public List<FixedPriceContractVM>? AllFixes { get; set; }
        public double? RolledVolume { get; set; }
        public int? CurrentTradeId { get; set; }
        public int CurrentAllocation { get; set; }
        public bool IsCurrentTrade { get; set; }


    }
    public class TexacoVolumes
    {
        public double? DieselBand7 { get; set; }
        public double? Unleaded { get; set; }
        public double? Adblue { get; set; }
        public double? SuperUnleaded { get; set; }
        public double? Diesel { get; set; }
    }
}