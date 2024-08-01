using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.InvoiceMethods;
using Fuelcards.Models;
using Fuelcards.Repositories;
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
                checks.InvoiceDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now));
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
        public JsonResult ConfirmInvoicing([FromBody] string Network)
        {
            try
            {
                InvoiceGenerator.GenerateXeroCSV(invoices);


                return Json("Success");
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
            // THIS IS TEMP
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

            summaryReport.DieselPrice = reportList.Sum(e => e.DieselPrice);
            summaryReport.PetrolPrice = reportList.Sum(e => e.PetrolPrice);
            summaryReport.LubesPrice = reportList.Sum(e => e.LubesPrice);
            summaryReport.GasoilPrice = reportList.Sum(e => e.GasoilPrice);
            summaryReport.AdbluePrice = reportList.Sum(e => e.AdbluePrice);
            summaryReport.PremDieselPrice = reportList.Sum(e => e.PremDieselPrice);
            summaryReport.SuperUnleadedPrice = reportList.Sum(e => e.SuperUnleadedPrice);
            summaryReport.BrushTollPrice = reportList.Sum(e => e.BrushTollPrice);
            summaryReport.TescoPrice = reportList.Sum(e => e.TescoPrice);
            summaryReport.OthersPrice = reportList.Sum(e => e.OthersPrice);

            summaryReport.Rolled = reportList.Sum(e => e.Rolled);
            summaryReport.Current = reportList.Sum(e => e.Current);
            summaryReport.RollAvailable = reportList.Sum(e => e.RollAvailable);
            summaryReport.DieselLifted = reportList.Sum(e => e.DieselLifted);
            summaryReport.Fixed = reportList.Sum(e => e.Fixed);
            summaryReport.Floating = reportList.Sum(e => e.Floating);
            summaryReport.TescoVol = reportList.Sum(e => e.TescoVol);
            summaryReport.NetTotal = reportList.Sum(e => e.NetTotal);
            summaryReport.Vat = reportList.Sum(e => e.Vat);
            summaryReport.Total = reportList.Sum(e => e.Total);
            // END




            try
            {
                throw new Exception("Error getting Png");
                return Json("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        [HttpPost]
        public JsonResult CompleteInvoicing([FromBody] CustomerInvoice customerInvoice)
        {
            InvoiceSummary summary = new();
            try
            {
                InvoicePDFModel newInvoice = new();
                
                if (customerInvoice.CustomerType == EnumHelper.CustomerType.Fix)
                {
                    newInvoice.fixedBox = summary.GetFixedDetails(customerInvoice);
                }
                newInvoice.rows = summary.ProductBreakdown(customerInvoice);
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
                
                
                newInvoice.InvoiceDate = DateOnly.FromDateTime(DateTime.Now);
                invoices.Add(newInvoice);

                reportList.Add(_report.CreateNewInvoiceReport(newInvoice));
                try
                {
                    InvoiceGenerator invoiceGenerator = new(newInvoice, _db);
                    FileHelperForInvoicing.CheckOrCorrectDirectorysBeforePDFCreation();
                    invoiceGenerator.generatePDF(newInvoice);
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
