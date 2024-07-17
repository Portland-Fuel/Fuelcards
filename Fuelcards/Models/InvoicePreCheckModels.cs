
using Fuelcards.Repositories;

namespace Fuelcards.Models
{
    public class InvoicePreCheckModels
    {
        private readonly IQueriesRepository _db;
        public InvoicePreCheckModels(IQueriesRepository db) 
        {
            _db = db;
        }
        public DateOnly invoiceDate { get; set; }
        public double? BasePrice { get => _db.GetBasePrice(invoiceDate); }
        public double?  PlattsPrice { get => BasePrice - 52.95; }
        public int? KeyfuelImports { get => _db.GetTotalEDIs(0); }
        public int? UkfuelImports { get => _db.GetTotalEDIs(1); }
        public int? TexacoImports { get => _db.GetTotalEDIs(2); }
        public double? TexacoDieselBand7 { get => _db.GetDieselBand7Texaco(); }
        public List<CustomerInvoice>? keyfuelsInvoiceList { get => _db.GetCustomersToInvoice(0, invoiceDate); }

        public List<CustomerInvoice>? ukFuelInvoiceList { get => _db.GetCustomersToInvoice(1, invoiceDate); }
        public List<CustomerInvoice>? texacoInvoiceList { get => _db.GetCustomersToInvoice(2, invoiceDate); }
        public List<CustomerInvoice>? fuelgenieInvoiceList { get; set; }
        public List<int> FailedKeyfuelsSites { get => _db.GetFailedSiteBanding(0); }
        public List<int> FailedUkfuelSites { get => _db.GetFailedSiteBanding(1); }
        public List<int> FailedTexacoSites { get => _db.GetFailedSiteBanding(2); }
    }
    public class CustomerInvoice
    {
        public string? name { get; set; }
        public double? addon { get; set; }
    }

}
