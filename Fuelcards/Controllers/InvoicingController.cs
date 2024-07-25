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
        public JsonResult GetInvoicePng([FromBody] CustomerInvoice customerInvoice)
        {
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
            try
            {
                //do Transaction faff


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
                if (transactionDataFromView.fixedInformation is not null)
                {
                    string egg = "Working";
                }
                EnumHelper.Network network = _db.getNetworkFromAccount((int)transactionDataFromView.account);
                TransactionBuilder tb = new(_sites, _db);

                DataToPassBack dataToPassBack = new();
                dataToPassBack.SiteName = tb.getSiteName(transactionDataFromView.transaction.SiteCode, network);
                tb.processTransaction(transactionDataFromView, network);

                dataToPassBack.InvoicePrice = "test";
                dataToPassBack.UnitPrice = "1.2399";
                dataToPassBack.Product = "test";
                return Json(dataToPassBack);
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
            public FixedInformation? fixedInformation { get; set; }
            public GenericTransactionFile? transaction { get; set; }
        }
        [HttpGet]
        public async Task<IActionResult> GetInvoicePreCheckModel()
        {
            try
            {
                InvoicePreCheckModels checks = new();
                checks.InvoiceDate = DateOnly.FromDateTime(DateTime.Now);
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
