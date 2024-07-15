using Fuelcards.GenericClassFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using static Fuelcards.Models.CustomerDetailsModels;

namespace Fuelcards.Controllers
{
    public class CustomerDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SearchCustomer([FromBody] string CustomerName)
        
        {
  /*          List<string> PFLXeroCustomerNames = HomeController.PFLXeroCustomersData.Select(x => x.Name).ToList();
            List<string> FTCXeroCustomerNames = HomeController.FTCXeroCustomersData.Select(x => x.Name).ToList();

            PFLXeroCustomerNames.AddRange(FTCXeroCustomerNames);

            bool customerExists = PFLXeroCustomerNames.Contains(CustomerName, StringComparer.OrdinalIgnoreCase);

            if (!customerExists)
            {
                throw new Exception("Customer not found!!! Try reloading xero");
            }*/
            try
            {

               CustomerModel Return =  RetrieveCustomer.GetCustomerInformation(CustomerName);
                var JsonResult = new
                {
                    Addons = Return.allAddons,
                    CustomerName = Return.name,
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
