using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.InvoiceMethods;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Reflection.Metadata;

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
                checks.InvoiceDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now.AddDays(-7)));
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
        [HttpPost]
        public JsonResult UploadNewItemInventoryCode([FromBody] ItemCodeAndPorductData itemCodeAndPorductData)
        {
            _db.UploadNewItemInventoryCode(itemCodeAndPorductData.description, itemCodeAndPorductData.itemCode);
            return Json("True");
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
                var ListOfProducts = _db.GetListOfProducts();
                InvoiceGenerator.GenerateXeroCSV(invoices, ListOfProducts);

                return Json(new { Status = "Success" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;

                // Create an error response object
                var errorResponse = new
                {
                    Status = "Error",
                    ExceptionType = e.GetType().Name,
                    Message = e.Message,
                    Description = e.Message.Split(":")[1],
                    StackTrace = e.StackTrace // Optional: include stack trace for debugging
                };

                return Json(errorResponse);
            }
        }

        [HttpPost]
        public JsonResult GetInvoicePng([FromBody] CustomerInvoice customerInvoice)
        {
            // THIS IS TEMP

            foreach (var item in reportList)
            {
                Console.WriteLine(item.Total);
            }

            InvoiceReport summaryReport = new InvoiceReport();
            summaryReport.DieselVol = reportList.Sum(e => e.DieselVol);
            summaryReport.PetrolVol = reportList.Sum(e => e.PetrolVol);
            summaryReport.LubesVol = reportList.Sum(e => e.LubesVol);
            summaryReport.GasoilVol = reportList.Sum(e => e.GasoilVol);
            summaryReport.AdblueVol = reportList.Sum(e => e.AdblueVol);
            summaryReport.PremDieselVol = reportList.Sum(e => e.PremDieselVol);
            summaryReport.SuperUnleadedVol = reportList.Sum(e => e.SuperUnleadedVol);
            summaryReport.BrushTollVol = reportList.Sum(e => e.BrushTollVol);
            summaryReport.TescoVol = reportList.Sum(e => e.TescoVol);
            summaryReport.OtherVol = reportList.Sum(e => e.OtherVol);

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
                EnumHelper.Network network = _db.getNetworkFromAccount((int)customerInvoice.account);
                InvoicePDFModel newInvoice = new();
                
                if (customerInvoice.CustomerType != EnumHelper.CustomerType.Floating)
                {
                    newInvoice.fixedBox = summary.GetFixDetails(customerInvoice, network, customerInvoice.CustomerType);
                }
                newInvoice.rows = summary.ProductBreakdown(customerInvoice, network);
                newInvoice.transactions = summary.TurnsTransactionsToPdf(customerInvoice.CustomerTransactions);
                newInvoice.totals = summary.GetInvoiceTotal(newInvoice.rows);
                if(customerInvoice.name != "The Fuel Trading Company")
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
                    newInvoice = CheckFileName(newInvoice);
                    InvoiceGenerator invoiceGenerator = new(newInvoice);
                    FileHelperForInvoicing.CheckOrCorrectDirectorysBeforePDFCreation();
                    invoiceGenerator.generatePDF(newInvoice);
                    //invoiceGenerator.generatePDFImage(newInvoice);
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

        private InvoicePDFModel CheckFileName(InvoicePDFModel newInvoice)
        {
            string[] errorChars = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };

            foreach (var errorChar in errorChars)
            {
                if (newInvoice.CustomerDetails.CompanyName.Contains(errorChar))
                {
                    newInvoice.CustomerDetails.CompanyName = newInvoice.CustomerDetails.CompanyName.Replace(errorChar, string.Empty);
                }
            }

            return newInvoice;
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
