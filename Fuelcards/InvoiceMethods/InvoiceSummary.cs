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

            var ProductsUsed = invoice.CustomerTransactions?.GroupBy(t => t.ProductCode).Select(g => g.First());
            foreach (var product in ProductsUsed)
            {
                SummaryRow newRow = new()
                {
                    productCode = (int)product.ProductCode,
                    productName = EnumHelper.GetProductFromProductCode(product.ProductCode, EnumHelper.NetworkEnumFromInt(product.network)).ToString(),
                    Quantity = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.CustomerTransactions.Where(e => e.ProductCode == product.ProductCode).Sum(e => e.Quantity)), 2, MidpointRounding.AwayFromZero)),
                    NetTotal = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.CustomerTransactions.Where(e => e.ProductCode == product.ProductCode).Sum(e => e.InvoicePrice)), 2, MidpointRounding.AwayFromZero)),
                    VAT = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.CustomerTransactions.Where(e => e.ProductCode == product.ProductCode).Sum(e => e.InvoicePrice) * 0.2), 2, MidpointRounding.AwayFromZero))
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
                    CardNumber = item.CardNumber,
                    TransactionNumber = item.TransactionNumber.ToString(),
                    RegNo = item.PrimaryRegistration,
                    SiteName = item.SiteName,
                    TranDate = item.TransactionDate,
                    TranTime = item.TransactionTime,
                    product = item.Product,
                    UnitPrice = item.UnitPrice,
                    Volume = item.Quantity,
                    Value = item.InvoicePrice,
                };
                if (transactionsPDF.product == "TescoDieselNewDiesel") transactionsPDF.product = "Retail Diesel";
                pdfList.Add(transactionsPDF);
            }
            return pdfList;
        }

        internal CustomerDetails GetCustomerDetails(CustomerInvoice customerInvoice, IQueriesRepository _db, string xeroID)
        {
            int? terms = _db.GetPaymentTerms(xeroID);
            CustomerDetails details = new();
            details.CompanyName = customerInvoice.name;
            details.account = customerInvoice.account;
            details.paymentDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now)).AddDays((int)terms);
            return details;
        }
        public FixedBox GetFixedDetails(CustomerInvoice customerInvoice)
        {
            FixedBox fixedBox = new();
            fixedBox.TotalDieselVolumeLiftedOnThisInvoice = Convert.ToDouble(Math.Round(Convert.ToDecimal(customerInvoice.CustomerTransactions.Where(e=>e.Product == "Diesel").Sum(e=>e.Quantity)), 2));
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