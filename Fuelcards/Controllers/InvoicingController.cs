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
                throw new Exception("This is a test exception");    
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
        public IActionResult GetInvoicePreCheckModel()
        {
            try
            {
                InvoicePreCheckModels checks = new(_db);
                checks.texacoVolume = new(_db);
                //var egg = checks.keyfuelsInvoiceList;
                return Json(checks);
            }
            catch (Exception e)
            {
                
                throw;
            }
        }

    }
}
