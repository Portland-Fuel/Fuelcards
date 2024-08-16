using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.Graph;
using System.Linq.Expressions;

namespace Fuelcards.InvoiceMethods
{
    public class InvoiceSummary
    {
        public List<SummaryRow> ProductBreakdown(CustomerInvoice invoice, EnumHelper.Network network)
        {
            var (transactionsByProduct, dieselTransactionsByBand) = UkFuelsVersion(invoice, network);
            List<SummaryRow> Rows = new();

            // Process transactions for each product
            foreach (var product in transactionsByProduct)
            {
                // Exclude Diesel from general product processing to handle separately by bands
                if (!product.Key.Equals("Diesel"))
                {
                    SummaryRow newRow = new()
                    {
                        productCode = (int)product.Value.First().productCode,
                        productName = EnumHelper.GetProductFromProductCode(product.Value.First().productCode, EnumHelper.NetworkEnumFromInt(product.Value.First().network)).ToString(),
                        Quantity = Convert.ToDouble(Math.Round(Convert.ToDecimal(product.Value.Sum(e => e.quantity)), 2, MidpointRounding.AwayFromZero)),
                        NetTotal = Convert.ToDouble(Math.Round(Convert.ToDecimal(product.Value.Sum(e => e.invoicePrice)), 2, MidpointRounding.AwayFromZero)),
                        VAT = Convert.ToDouble(Math.Round(Convert.ToDecimal(product.Value.Sum(e => e.invoicePrice) * 0.2), 2, MidpointRounding.AwayFromZero))
                    };

                    newRow.Quantity = TransactionBuilder.ConvertToLitresBasedOnNetwork(newRow.Quantity, network);
                    Rows.Add(newRow);
                }
            }

            // Process Diesel transactions by bands, each band gets its own row
            foreach (var band in dieselTransactionsByBand)
            {
                SummaryRow dieselRow = new()
                {
                    productCode = band.Value.First().productCode.HasValue ? (int)band.Value.First().productCode : 0,
                    productName = "Diesel - " + band.Key,
                    Quantity = Convert.ToDouble(Math.Round(Convert.ToDecimal(band.Value.Sum(t => t.quantity)), 2, MidpointRounding.AwayFromZero)),
                    NetTotal = Convert.ToDouble(Math.Round(Convert.ToDecimal(band.Value.Sum(t => t.invoicePrice)), 2, MidpointRounding.AwayFromZero)),
                    VAT = Convert.ToDouble(Math.Round(Convert.ToDecimal(band.Value.Sum(t => t.invoicePrice) * 0.2), 2, MidpointRounding.AwayFromZero))
                };
                dieselRow.Quantity = TransactionBuilder.ConvertToLitresBasedOnNetwork(dieselRow.Quantity, network);
                if (dieselRow.productName == "Diesel - " && invoice.CustomerType == EnumHelper.CustomerType.ExpiredFixWithVolume)
                {
                    dieselRow.productName = "Diesel";
                    dieselRow.Quantity = dieselRow.Quantity - DieselTransaction.RolledVolumeUsedOnThisInvoice;
                }
                // Check for specific business rules related to diesel (if applicable)
                if (invoice.CustomerType == EnumHelper.CustomerType.Fix)
                {
                    dieselRow = AppendRowForFix(dieselRow, invoice); // Placeholder for actual method if required
                }

                Rows.Add(dieselRow);
            }

            return Rows;
        }

        private (Dictionary<string, List<GenericTransactionFile>> transactionsByProduct, Dictionary<string, List<GenericTransactionFile>> dieselTransactionsByBand) UkFuelsVersion(CustomerInvoice invoice, EnumHelper.Network network)
        {
            string bandKey = string.Empty;
            // Initialize dictionaries to hold transactions by product and bands for Diesel
            Dictionary<string, List<GenericTransactionFile>> transactionsByProduct = new Dictionary<string, List<GenericTransactionFile>>();
            Dictionary<string, List<GenericTransactionFile>> dieselTransactionsByBand = new Dictionary<string, List<GenericTransactionFile>>();

            // Check if there are any transactions
            if (invoice.CustomerTransactions == null || !invoice.CustomerTransactions.Any())
            {
                Console.WriteLine("No transactions to process.");
                return (transactionsByProduct, dieselTransactionsByBand); // Return empty dictionaries if no transactions
            }

            // Process each transaction
            foreach (var transaction in invoice.CustomerTransactions)
            {
                // Group by product
                if (!transactionsByProduct.ContainsKey(transaction.product))
                {
                    transactionsByProduct[transaction.product] = new List<GenericTransactionFile>();
                }
                transactionsByProduct[transaction.product].Add(transaction);
                
                // Further group Diesel by band
                if (transaction.product == "Diesel")
                {
                    if(network == EnumHelper.Network.UkFuel)
                    {
                        bandKey = transaction.band switch
                        {
                            "8" => "Sainsburys",
                            "9" => "Tesco",
                            _ => ""
                        };

                    }
                    if (network == EnumHelper.Network.Keyfuels)
                    {
                        bandKey = transaction.productCode.ToString() switch
                        {
                            "70" => "Retail",
                            "77" => "Tesco",
                            _ => ""
                        };

                    }
                    
                    if (!dieselTransactionsByBand.ContainsKey(bandKey))
                    {
                        dieselTransactionsByBand[bandKey] = new List<GenericTransactionFile>();
                    }
                    dieselTransactionsByBand[bandKey].Add(transaction);
                }
            }
            foreach (var product in transactionsByProduct)
            {
                Console.WriteLine($"Product: {product.Key}, Transactions: {product.Value.Count}");
            }
            foreach (var band in dieselTransactionsByBand)
            {
                Console.WriteLine($"Diesel {band.Key}, Transactions: {band.Value.Count}");
            }

            return (transactionsByProduct, dieselTransactionsByBand);
        }

        private SummaryRow AppendRowForFix(SummaryRow newRow, CustomerInvoice invoice)
        {
            double? FixVolume = invoice.fixedInformation.AllFixes.FirstOrDefault(e => e.Id == invoice.fixedInformation.CurrentTradeId)?.FixedVolume;
            double? FixPrice = invoice.fixedInformation.AllFixes.FirstOrDefault(e => e.Id == invoice.fixedInformation.CurrentTradeId)?.FixedPriceIncDuty;
            double? RemainingToCharge = Round2(DieselTransaction.FixedVolumeRemainingForCurrent);
            var total = invoice.CustomerTransactions.Where(e=>e.product == "Diesel").Sum(e => e.invoicePrice);
            double? PriceOfRemainingVolume = RemainingToCharge * 1.2302;


            if (RemainingToCharge > 0)
            {
                newRow.Quantity = FixVolume;
                double? RemainingVolumeCharge = RemainingToCharge * (FixPrice / 100);
                newRow.NetTotal = Round2(total + RemainingVolumeCharge);
                newRow.VAT = Round2(newRow.NetTotal * 0.2);
            }
            return newRow;

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
                    Band = item.band,
                    productCode = item.productCode,

                };
                if (transactionsPDF.product == "TescoDieselNewDiesel") transactionsPDF.product = "Retail Diesel";
                pdfList.Add(transactionsPDF);
            }
            return pdfList;
        }

        internal CustomerDetails GetCustomerDetails(CustomerInvoice customerInvoice, IQueriesRepository _db, string xeroID, int network)
        {
            int? terms = _db.GetPaymentTerms(xeroID);
            if (!terms.HasValue) throw new ArgumentException($"There are no payment terms for this customer - {customerInvoice.name}");
            CustomerDetails details = new();
            details.CompanyName = customerInvoice.name;
            
            details.account = customerInvoice.account;
            details.paymentDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now)).AddDays((int)terms);
            details.Network = EnumHelper.NetworkEnumFromInt(network).ToString();
            var address = HomeController.PFLXeroCustomersData.Where(e => e.Name == details.CompanyName).FirstOrDefault();
            details.AddressArr = GetAddress(address, xeroID);
            details.InvoiceNumber = _db.getNewInvoiceNumber(network);
            details.InvoiceType = _db.GetInvoiceDisplayGroup(details.CompanyName, details.Network);
            return details;
        }

        private string[] GetAddress(Xero.NetStandard.OAuth2.Model.Accounting.Contact? address, string XeroId)
        {
            if (XeroId == "FTC")
            {
                return new string[] { "1 Toft Green", null, "York", null, "YO1 6JT" };

            }
            else if (address.Addresses[0].AddressLine1 is not null)
            {
                return new string[] { address.Addresses[0].AddressLine1, address.Addresses[0].AddressLine2, address.Addresses[0].City, address.Addresses[0].Country, address.Addresses[0].PostalCode };
            }
            else
            {
                return new string[] { address.Addresses[1].AddressLine1, address.Addresses[1].AddressLine2, address.Addresses[1].City, address.Addresses[1].Country, address.Addresses[1].PostalCode };

            }
        }

        public FixedBox GetFixDetails(CustomerInvoice customerInvoice, EnumHelper.Network network, EnumHelper.CustomerType custType)
        {

            FixedBox fixedBox = new();
            fixedBox.TotalDieselVolumeLiftedOnThisInvoice = Round2(customerInvoice.CustomerTransactions.Where(e => e.product == "Diesel").Sum(e => e.quantity));
            fixedBox.TotalDieselVolumeLiftedOnThisInvoice = TransactionBuilder.ConvertToLitresBasedOnNetwork(fixedBox.TotalDieselVolumeLiftedOnThisInvoice, network);
            if (custType == EnumHelper.CustomerType.Fix)
            {
                fixedBox.FixedPriceVolumeForThisPeriod = customerInvoice.fixedInformation.AllFixes.Where(e => e.Id == customerInvoice.fixedInformation.CurrentTradeId).FirstOrDefault()?.FixedVolume;
            }
            else
            {
                fixedBox.FixedPriceVolumeForThisPeriod = 0;
            }
            fixedBox.FixedPriceVolumeFromPreviousPeriods = customerInvoice.fixedInformation.RolledVolume;
            fixedBox.FixedPriceVolumeUsedOnThisinvoice = Round2(DieselTransaction.FixedVolumeUsedOnThisInvoice);
            fixedBox.FixedPriceRemaining = Round2(DieselTransaction.FixedVolumeRemainingForCurrent) + DieselTransaction.AvailableRolledVolume;
            return fixedBox;
        }
        public static double? Round2(double? input)
        {
            return Convert.ToDouble(Math.Round(Convert.ToDecimal(input), 2));
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
        public CustomerDetails CustomerDetails { get; set; }
        public List<SummaryRow> rows { get; set; }
        public InvoiceTotals totals { get; set; }
        public List<TransactionsPDF> transactions { get; set; }
        public FixedBox fixedBox { get; set; }
        public EnumHelper.Network network { get; set; }
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
        public string? Band { get; set; }
        public int? productCode { get; set; }
        public double? Commission { get; set; }
    }
    public class CustomerDetails
    {
        public string CompanyName { get; set; }
        public int? account { get; set; }
        public DateOnly paymentDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public int InvoiceType { get; set; }
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
        public int? TradeId { get; set; }

    }

}