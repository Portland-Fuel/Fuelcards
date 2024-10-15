using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.Graph;
using System.Linq.Expressions;
using Xero.NetStandard.OAuth2.Model.Accounting;

namespace Fuelcards.InvoiceMethods
{
    public class InvoiceSummary
    {
        public List<SummaryRow> ProductBreakdown(CustomerInvoice invoice, EnumHelper.Network network)
        {
            var (transactionsByProduct, dieselTransactionsByBand) = ProductSplitter(invoice, network);
            List<SummaryRow> Rows = new();

            foreach (var product in transactionsByProduct)
            {
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
            foreach (var band in dieselTransactionsByBand.OrderByDescending(b => b.Key == "" ? 1 : 0))
            {
                SummaryRow newRow = new();

                newRow.productCode = (int)band.Value.First().productCode;
                newRow.productName = "Diesel - " + band.Key;
                if (newRow.productName == "Diesel - ") newRow.productName = "Diesel";
                newRow.Quantity = TransactionBuilder.ConvertToLitresBasedOnNetwork(Convert.ToDouble(Math.Round(Convert.ToDecimal(band.Value.Sum(e => e.quantity)), 2, MidpointRounding.AwayFromZero)),network);
                newRow.NetTotal = Convert.ToDouble(Math.Round(Convert.ToDecimal(band.Value.Sum(e => e.invoicePrice)), 2, MidpointRounding.AwayFromZero));
                newRow.VAT = Convert.ToDouble(Math.Round(Convert.ToDecimal(band.Value.Sum(e => e.invoicePrice) * 0.2), 2, MidpointRounding.AwayFromZero));
                Rows.Add(newRow);
            }
            CheckForExcessVolume(invoice.fixedInformation, Rows);
            return Rows;
        }

        private void CheckForExcessVolume(FixedInformation fix, List<SummaryRow> Rows)
        {
            if (fix is null || DieselTransaction.FixedVolumeRemainingForCurrent <= 0) return;
            double? FixVolume = fix.AllFixes.FirstOrDefault(e => e.Id == fix.CurrentTradeId)?.FixedVolume;
            double? FixPrice = fix.AllFixes.FirstOrDefault(e => e.Id == fix.CurrentTradeId)?.FixedPriceIncDuty;
            SummaryRow ExcessRow = new();
            ExcessRow.productName = "Fixed Remaining Diesel";
            ExcessRow.Quantity = Round2(DieselTransaction.FixedVolumeRemainingForCurrent);
            ExcessRow.NetTotal = Round2(ExcessRow.Quantity * (FixPrice / 100));
            ExcessRow.VAT = Round2((ExcessRow.NetTotal * 0.2));
            ExcessRow.productCode = 1;
            Rows.Add(ExcessRow);
        }

        private (Dictionary<string, List<GenericTransactionFile>> transactionsByProduct, Dictionary<string, List<GenericTransactionFile>> dieselTransactionsByBand) ProductSplitter(CustomerInvoice invoice, EnumHelper.Network network)
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
                            "3" => "Morrisons",
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
                    if (network == EnumHelper.Network.Texaco)
                    {
                        bandKey = transaction.band.ToString() switch
                        {
                            "8" => "Sainsburys",
                            "9" => "Tesco",
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

        private SummaryRow AppendRowForFix(SummaryRow newRow, CustomerInvoice invoice, List<SummaryRow> rows)
        {
            if (invoice.fixedInformation.AllFixes.Where(e => e.Id == invoice.fixedInformation.CurrentTradeId).Select(e => e.FrequencyId).Any(frequencyId => frequencyId != 3))
            {
                var RowsToInclude = rows.Where(e => e.productName.Contains("Diesel") || e.productName.Contains("Morrisons"));


                double? FixVolume = invoice.fixedInformation.AllFixes.FirstOrDefault(e => e.Id == invoice.fixedInformation.CurrentTradeId)?.FixedVolume;
                double? FixPrice = invoice.fixedInformation.AllFixes.FirstOrDefault(e => e.Id == invoice.fixedInformation.CurrentTradeId)?.FixedPriceIncDuty;
                double? RemainingToCharge = Round2(DieselTransaction.FixedVolumeRemainingForCurrent);
                var total = invoice.CustomerTransactions.Where(e => e.product == "Diesel").Sum(e => e.invoicePrice);
                if (RemainingToCharge > 0)
                {
                    newRow.Quantity = FixVolume;
                    //newRow.Quantity = newRow.Quantity + RemainingToCharge;

                    double? RemainingVolumeCharge = RemainingToCharge * (FixPrice / 100);
                    newRow.NetTotal = Round2(total + RemainingVolumeCharge);
                    //newRow.NetTotal = Round2(newRow.NetTotal - rows.Sum(e => e.NetTotal));
                    newRow.VAT = Round2(newRow.NetTotal * 0.2);
                }
                return newRow;
            }
            return newRow;
        }

        public InvoiceTotals GetInvoiceTotal(List<SummaryRow> rows)
        {
            InvoiceTotals total = new InvoiceTotals()
            {
                Goods = InvoiceSummary.Round2(rows.Sum(e => e.NetTotal)),
                VAT = InvoiceSummary.Round2(rows.Sum(e => e.VAT)),
            };
            total.Total = InvoiceSummary.Round2(total.Goods + total.VAT);
            Console.WriteLine(total.Total);
            return total;
        }
        public List<TransactionsPDF> TurnsTransactionsToPdf(EnumHelper.Network network,CustomerInvoice customerInvoice, IQueriesRepository _db)
        {
            List<TransactionsPDF> pdfList = new();
            foreach (var item in customerInvoice.CustomerTransactions)
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
                    Volume =  TransactionBuilder.ConvertToLitresBasedOnNetwork(item.quantity, network),
                    Value = item.invoicePrice,
                    Mileage = item.mileage.ToString(),
                    Band = item.band,
                    productCode = item.productCode,

                };
                if (transactionsPDF.product == "TescoDieselNewDiesel") transactionsPDF.product = "Retail Diesel";

                double? addon = _db.GetAddonForSpecificTransaction(_db.GetPortlandIdFromAccount((int)customerInvoice.account).Result, item.transactionDate, network, customerInvoice.IfuelsCustomer, (int)customerInvoice.account);

                transactionsPDF.Commission = IfuelCommission.CalculateCommission(network, customerInvoice, item,addon);

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
        public string VATRate = "20%";
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