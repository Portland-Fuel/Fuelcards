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
        public JsonResult SubmitAddOrEdit([FromBody] NewCustomerDetailsModel.AddEditCustomerFormData AddEditCustomerFormData)
        {
            try
            {
                if(AddEditCustomerFormData.isUpdateCustomer == true)
                {
                    SortChanges(AddEditCustomerFormData.keyFuelsInfo, AddEditCustomerFormData.customerName, EnumHelper.Network.Keyfuels);
                    SortChanges(AddEditCustomerFormData.uKFuelsInfo, AddEditCustomerFormData.customerName, EnumHelper.Network.UkFuel);
                    SortChanges(AddEditCustomerFormData.texacoInfo, AddEditCustomerFormData.customerName, EnumHelper.Network.Texaco);
                }
                else
                {
                    var newCustomer = "PLEASE CODE THIS";
                }
                return Json("JsonResult");
            }
            catch (Exception e)
            {
                Response.StatusCode = 443;
                return Json(new { error = e.Message });
            }
        }

        private void SortChanges(NewCustomerDetailsModel.NetworkInfo? networkInfo, string CustomerName, EnumHelper.Network network)
        {
            foreach (var NewAddon in networkInfo.newAddons)
            {
                _db.UpdateAddon(NewAddon, CustomerName, network);
            }
            foreach (var UpdatedAccount in networkInfo.newAccountInfo)
            {
                _db.UpdateAccount(UpdatedAccount,CustomerName, network);
            }
            foreach (var newFix in networkInfo.newFixesForCustomer)
            {
                _db.NewFix(newFix, CustomerName, network);
            }
        }
    }
}
