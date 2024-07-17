using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Fuelcards.Models;
//using static Fuelcards.Models.CustomerDetailsModels;

namespace Fuelcards.Controllers
{
    public class CustomerDetailsController : Controller
    {
        private readonly IQueriesRepository _db;
        public CustomerDetailsController(IQueriesRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SearchCustomer([FromBody] string xeroId)
        {
            RetrieveCustomer customerClass = new(_db);
            try
            {

                CustomerModel Return = customerClass.GetCustomerInformation(xeroId);
                var JsonResult = new
                {
                    networkData = Return.networks,
                    CustomerName = Return.name,
                    portlandId = Return.portlandId,
                    XeroId = Return.xeroID,
                    adress = Return.address,


                };

                return Json(JsonResult);

            }
            catch (Exception e)
            {
                Response.StatusCode = 443;
                return Json(new { error = e.Message });
            }
        }


        [HttpPost]
        public JsonResult SubmitAddOrEdit([FromBody] CustomerDetailsModels.AddEditCustomerFormData AddEditCustomerFormData)
        {
            try
            {
                var JsonResult = new
                {
                    ffff = "f" +
                    "ff"
                };

                return Json(JsonResult);
            }
            catch (Exception e)
            {
                Response.StatusCode = 443;
                return Json(new { error = e.Message });
            }
        }

        [HttpPost]

        public JsonResult UpdateAddon([FromBody] AddonFromJs customerPricingAddon)
        {
            try
            {
                _db.UpdateAddon(customerPricingAddon);
                
                    
                return Json("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 443;
                return Json(new { error = e.Message });
            }
        }

    }

    public struct AddonFromJs
    {
        public string? account { get; set; }
        public string addon { get; set; }
        public string? effectiveFrom { get; set; }
        public string toEmail { get; set; }
        public string ccEmail
        { get; set; }
        public string bccEmail { get; set; }

    }
}
