using DataAccess.Fuelcards;
using Fuelcards.CustomExceptions;
using Fuelcards.GenericClassFiles;
using Fuelcards.InvoiceMethods;
using Fuelcards.Models;
using Fuelcards.Repositories;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fuelcards.Controllers
{
    public class InvoicingController : Controller
    {
        private readonly IQueriesRepository _db;
        private readonly List<SiteNumberToBand> _sites;
        public static List<InvoicePDFModel> invoices = new();
        public static InvoiceReporter _report = new();
        public static List<InvoiceReport> reportList = new();
        public InvoicingController(IQueriesRepository db)
        {
            _db = db;
            _sites = _db.GetAllSiteInformation();
        }


        public IActionResult Invoicing()
        {
            // Return the view without the model, as the model will be fetched via AJAX if needed
            return View("/Views/Invoicing/RealInvoicing.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> GetInvoicePreCheckModel()
        {
            try
            {
                InvoicePreCheckModels checks = new();
                checks.InvoiceDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now.AddDays(-11)));
                checks.BasePrice = _db.GetBasePrice(checks.InvoiceDate);
                checks.PlattsPrice = checks.BasePrice - 52.95;
                checks.KeyfuelImports = _db.GetTotalEDIs(0);
                checks.UkfuelImports = _db.GetTotalEDIs(1);
                checks.TexacoImports = _db.GetTotalEDIs(2);
                checks.KeyfuelsInvoiceList = await _db.GetCustomersToInvoice(0, checks.InvoiceDate, checks.BasePrice);
                checks.UkFuelInvoiceList = await _db.GetCustomersToInvoice(1, checks.InvoiceDate, checks.BasePrice);
                checks.TexacoInvoiceList = await _db.GetCustomersToInvoice(2, checks.InvoiceDate, checks.BasePrice);
                checks.FailedKeyfuelsSites = await _db.GetFailedSiteBanding(0);
                checks.FailedUkfuelSites = await _db.GetFailedSiteBanding(1);
                checks.FailedTexacoSites = await _db.GetFailedSiteBanding(2);
                checks.TexacoVolume = new();
                checks.TexacoVolume.DieselBand7 = _db.GetDieselBand7Texaco();
                checks.TexacoVolume.Unleaded = _db.GetProductVolume(EnumHelper.Products.ULSP);
                checks.TexacoVolume.Adblue = _db.GetProductVolume(EnumHelper.Products.Adblue);
                checks.TexacoVolume.SuperUnleaded = _db.GetProductVolume(EnumHelper.Products.SuperUnleaded);
                checks.TexacoVolume.Diesel = _db.GetProductVolume(EnumHelper.Products.Diesel);
                checks.KeyfuelsDuplicates = _db.CheckForDuplicateTransactions(EnumHelper.Network.Keyfuels);
                checks.UkFuelDuplicates = _db.CheckForDuplicateTransactions(EnumHelper.Network.UkFuel);
                checks.TexacoDuplicates = _db.CheckForDuplicateTransactions(EnumHelper.Network.Texaco);

                return Json(checks);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult GetEmailBody([FromBody] CustomerInvoice customerInvoice)
        {
            try
            {
                throw new Exception("Error getting Email body");

                return Json("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }

        public struct ItemCodeAndPorductData
        {
            public string description { get; set; }
            public string itemCode { get; set; }
        }
        [HttpPost]
        public JsonResult ConfirmInvoicing([FromBody] string Network)
        {
            try
            {
                try
                {
                    var ListOfProducts = _db.GetListOfProducts();
                    InvoiceGenerator.GenerateXeroCSV(invoices);
                }
                catch (Exception e)
                {
                    throw new InventoryItemCodeNotInDb(e.Message.Split(';')[1]);
                }
                _db.ConfirmChanges(Network, reportList.Where(e => e.Network == (int)EnumHelper.NetworkEnumFromString(Network)).ToList(), invoices.Where(e => e.network.ToString() == Network).ToList());

                return Json("Success");
            }
            catch (InventoryItemCodeNotInDb e)
            {
                var res = new
                {
                    exceptionType = "InventoryItemCodeNotInDb",
                    message = "Inventory Item Code not in database",
                    description = e.Message
                };
                Response.StatusCode = 500;
                return Json(res);
            }


            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }

        }

        [HttpPost]
        public JsonResult GetInvoicePng([FromBody] CustomerInvoice customerInvoice)
        {
            try
            {
                var netTotal = reportList.Sum(e => e.NetTotal);
                var DieselLifted = reportList.Sum(e => e.DieselLifted);
                var TescoPrice = reportList.Sum(e => e.TescoPrice);
                var TTotal = reportList.Sum(e => e.Total);
                Console.Clear();
                foreach (var item in reportList)
                {
                    Console.WriteLine(item.Total);
                }
                var invoice = invoices.Where(e => e.CustomerDetails.CompanyName == customerInvoice.name).FirstOrDefault();
                if (invoice == null)
                {
                    throw new Exception("Invoice not found");
                }

                string fileName = FileHelperForInvoicing.BuildingFileNameForInvoicing(invoice, invoice.CustomerDetails.CompanyName);
                string path = Path.Combine(
                    FileHelperForInvoicing._startingDirectory,
                    FileHelperForInvoicing._year.ToString(),
                    invoice.InvoiceDate.ToString("yyMMdd"),
                    GetNetworkName(invoice.CustomerDetails.Network),
                    "PDFImages",
                    fileName + ".png"
                );

                return Json(path);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error: Could not get PNG " + e.Message);
            }
        }

        public string GetNetworkName(string Network)
        {
            switch (Network)
            {
                case "Texaco":
                    return "FastFuel";
                default:
                    return Network;
            }

        }
        [HttpPost]
        public JsonResult CompleteInvoicing([FromBody] CustomerInvoice customerInvoice)
        {
            InvoiceSummary summary = new();
            try
            {
                InvoicePDFModel newInvoice = new();
                newInvoice.network = _db.getNetworkFromAccount((int)customerInvoice.account);

                newInvoice.InvoiceDate = customerInvoice.invoiceDate;
                if (customerInvoice.CustomerType != EnumHelper.CustomerType.Floating)
                {
                    newInvoice.fixedBox = summary.GetFixDetails(customerInvoice, newInvoice.network, customerInvoice.CustomerType);
                }
                
                newInvoice.rows = summary.ProductBreakdown(customerInvoice, newInvoice.network);
                newInvoice.transactions = summary.TurnsTransactionsToPdf(customerInvoice.CustomerTransactions);
                newInvoice.totals = summary.GetInvoiceTotal(newInvoice.rows);
                if (customerInvoice.name != "The Fuel Trading Company")
                {
                    newInvoice.CustomerDetails = summary.GetCustomerDetails(customerInvoice, _db, HomeController.PFLXeroCustomersData.Where(e => e.Name == customerInvoice.name).FirstOrDefault().ContactID.ToString(), (int)customerInvoice.CustomerTransactions[0].network);
                }
                
                else
                {
                    newInvoice.CustomerDetails = summary.GetCustomerDetails(customerInvoice, _db, "FTC", (int)customerInvoice.CustomerTransactions[0].network);
                }

                invoices.Add(newInvoice);

                reportList.Add(_report.CreateNewInvoiceReport(newInvoice));
                try
                {
                    InvoiceGenerator invoiceGenerator = new(newInvoice);
                    FileHelperForInvoicing.CheckOrCorrectDirectorysBeforePDFCreation();
                    invoiceGenerator.generatePDF(newInvoice);
                    invoiceGenerator.generatePDFImage(newInvoice);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return Json("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        [HttpPost]
        public JsonResult SendEmail([FromBody] SendEmailInformation sendEmailInformation)
        {
            try
            {
                throw new Exception("Error sending email");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }


        [HttpPost]
        public JsonResult ProcessTransactionFromPage([FromBody] TransactionDataFromView transactionDataFromView)
        {
            try
            {
                EnumHelper.Network network = _db.getNetworkFromAccount((int)transactionDataFromView.account);
                TransactionBuilder tb = new(_sites, _db);
                DataToPassBack model = tb.processTransaction(transactionDataFromView, network);
                return Json(model);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        public struct DataToPassBack
        {
            public string? SiteName { get; set; }
            public string? InvoicePrice { get; set; }
            public string? UnitPrice { get; set; }
            public string? Product { get; set; }
        }
        public struct TransactionDataFromView
        {
            public DateOnly invoiceDate { get; set; }
            public string? name { get; set; }
            public double? addon { get; set; }
            public int? account { get; set; }
            public EnumHelper.CustomerType customerType { get; set; }
            public bool IfuelsCustomer { get; set; }
            public FixedInformation? fixedInformation { get; set; }
            public GenericTransactionFile? transaction { get; set; }
        }
        public struct SendEmailInformation
        {
            public EmailDetails EmailDetails { get; set; }
            public CustomerInvoice CustomerInvoice { get; set; }
        }
        public struct EmailDetails
        {
            public string? htmlbody { get; set; }
            public string emailTo { get; set; }
            public string emailCc { get; set; }
            public string emailBcc { get; set; }
            public string emailSubject { get; set; }

        }
    }
}