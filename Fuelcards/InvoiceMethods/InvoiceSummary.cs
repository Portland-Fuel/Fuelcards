using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.Graph;

namespace Fuelcards.InvoiceMethods
{
    public class InvoiceSummary
    {
        public List<SummaryRow> ProductBreakdown(CustomerInvoice invoice)
        {
            List<SummaryRow> Rows = new();

            var ProductsUsed = invoice.CustomerTransactions?.GroupBy(t => t.productCode).Select(g => g.First());
            foreach (var product in ProductsUsed)
            {
                SummaryRow newRow = new()
                {
                    productCode = (int)product.productCode,
                    productName = EnumHelper.GetProductFromProductCode(product.productCode, EnumHelper.NetworkEnumFromInt(product.network)).ToString(),
                    Quantity = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.CustomerTransactions.Where(e => e.productCode == product.productCode).Sum(e => e.quantity)), 2, MidpointRounding.AwayFromZero)),
                    NetTotal = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.CustomerTransactions.Where(e => e.productCode == product.productCode).Sum(e => e.invoicePrice)), 2, MidpointRounding.AwayFromZero)),
                    VAT = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.CustomerTransactions.Where(e => e.productCode == product.productCode).Sum(e => e.invoicePrice) * 0.2), 2, MidpointRounding.AwayFromZero))
                };
                if (newRow.productName == EnumHelper.Products.TescoDieselNewDiesel.ToString()) newRow.productName = "Diesel Retail";
                Rows.Add(newRow);
            }
            return Rows;
        }
        public InvoiceTotals GetInvoiceTotal(List<SummaryRow> rows)
        {
            InvoiceTotals total = new InvoiceTotals()
            {
                Goods = rows.Sum(e => e.NetTotal),
                VAT = rows.Sum(e => e.VAT),
            };
            total.Total = total.Goods + total.VAT;
            return total;
        }
        public List<TransactionsPDF> TurnsTransactionsToPdf(List<GenericTransactionFile> transactions)
        {
            List<TransactionsPDF> pdfList = new();
            foreach (var item in transactions)
            {
                TransactionsPDF transactionsPDF = new()
                {
                    CardNumber = item.cardNumber,
                    TransactionNumber = item.transactionNumber.ToString(),
                    RegNo = item.primaryRegistration,
                    SiteName = item.siteName,
                    TranDate = item.transactionDate,
                    TranTime = item.transactionTime,
                    product = item.product,
                    UnitPrice = item.unitPrice,
                    Volume = item.quantity,
                    Value = item.invoicePrice,
                    Mileage = item.mileage.ToString(),
                };
                if (transactionsPDF.product == "TescoDieselNewDiesel") transactionsPDF.product = "Retail Diesel";
                pdfList.Add(transactionsPDF);
            }
            return pdfList;
        }

        internal CustomerDetails GetCustomerDetails(CustomerInvoice customerInvoice, IQueriesRepository _db, string xeroID, int network)
        {
            int? terms = _db.GetPaymentTerms(xeroID);
            CustomerDetails details = new();
            details.CompanyName = customerInvoice.name;
            details.account = customerInvoice.account;
            details.paymentDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now)).AddDays((int)terms);
            details.Network = EnumHelper.NetworkEnumFromInt(network).ToString();
            var address = HomeController.PFLXeroCustomersData.Where(e=>e.Name == details.CompanyName).FirstOrDefault();
            details.AddressArr = new string[] { address.Addresses[0].AddressLine1, address.Addresses[0].AddressLine2, address.Addresses[0].City, address.Addresses[0].Country, address.Addresses[0].PostalCode };
            return details;
        }
        public FixedBox GetFixedDetails(CustomerInvoice customerInvoice)
        {
            FixedBox fixedBox = new();
            fixedBox.TotalDieselVolumeLiftedOnThisInvoice = Convert.ToDouble(Math.Round(Convert.ToDecimal(customerInvoice.CustomerTransactions.Where(e=>e.product == "Diesel").Sum(e=>e.quantity)), 2));
            fixedBox.FixedPriceVolumeForThisPeriod = customerInvoice.fixedInformation.AllFixes.Where(e => e.Id == customerInvoice.fixedInformation.CurrentTradeId).FirstOrDefault()?.FixedVolume;
            fixedBox.FixedPriceVolumeFromPreviousPeriods = customerInvoice.fixedInformation.RolledVolume;
            fixedBox.FixedPriceVolumeUsedOnThisinvoice = Convert.ToDouble(Math.Round(Convert.ToDecimal(DieselTransaction.FixedVolumeUsedOnThisInvoice), 2));
            fixedBox.FixedPriceRemaining = Convert.ToDouble(Math.Round(Convert.ToDecimal(DieselTransaction.FixedVolumeRemainingForCurrent), 2)) + DieselTransaction.AvailableRolledVolume;
            return fixedBox;
        }
    }

    public class SummaryRow()
    {
        public int productCode { get; set; }
        public string productName { get; set; }
        public double? Quantity { get; set; }
        public double? NetTotal { get; set; }
        public double? VAT { get; set; }
    }
    public class InvoiceTotals
    {
        public double? Goods { get; set; }
        public double? VAT { get; set; }
        public double? Total { get; set; }
    }
    public class InvoicePDFModel
    {
        public DateOnly InvoiceDate { get; set; }
        public int InvoiceType { get; set;}
        public CustomerDetails CustomerDetails { get; set; }
        public List<SummaryRow> rows { get; set; }
        public InvoiceTotals totals { get; set; }
        public List<TransactionsPDF> transactions { get; set; }
        public FixedBox fixedBox { get; set; }
    }
    public class TransactionsPDF
    {
        public string CardNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string RegNo { get; set; }
        public string Mileage { get; set; }
        public string SiteName { get; set; }
        public DateOnly? TranDate { get; set; }
        public TimeOnly? TranTime { get; set; }
        public string product { get; set; }
        public double? UnitPrice { get; set; }
        public double? Volume { get; set; }
        public double? Value { get; set; }
    }
    public class CustomerDetails
    {
        public string CompanyName { get; set; }
        public int? account { get; set; }
        public DateOnly paymentDate { get; set; }
        public string? InvoiceNumber { get; set; }

        public string? Network { get; set; }

        public string[] AddressArr { get; set; }
    }
    public class FixedBox
    {
        public double? TotalDieselVolumeLiftedOnThisInvoice { get; set; }
        public double? FixedPriceVolumeForThisPeriod { get; set; }
        public double? FixedPriceVolumeFromPreviousPeriods { get; set; }
        public double? FixedPriceVolumeUsedOnThisinvoice { get; set; }
        public double? FixedPriceRemaining { get; set; }
    }

}