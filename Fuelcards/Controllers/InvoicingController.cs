using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Fuelcards.Controllers
{
    public class InvoicingController : Controller
    {
        private readonly IQueriesRepository _db;
        public InvoicingController(IQueriesRepository db)
        {
            _db = db;
        }


        public IActionResult Invoicing()
        {
            // Return the view without the model, as the model will be fetched via AJAX if needed
            return View("/Views/Invoicing/RealInvoicing.cshtml");
        }
        [HttpPost]
        public JsonResult InvoiceCustomer([FromBody] CustomerInvoice customer)
        {
            try
            {
                //do invoicing 

                return Json("True");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
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

                foreach (var item in checks.TexacoInvoiceList)
                {
                    if (item.name is null) { var egg = "egg"; }
                }
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

    }
}
