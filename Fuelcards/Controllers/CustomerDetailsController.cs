using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using static Fuelcards.Models.CustomerDetailsModels;

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
        public JsonResult SubmitAddOrEdit([FromBody] AddEditCustomerFormData AddEditCustomerFormData)
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

    }
}
