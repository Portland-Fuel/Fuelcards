
using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using PortlandXeroLib;
using System.Diagnostics;

namespace Fuelcards.Controllers
{
    public class HomeController : Controller
    {
        public static List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> PFLXeroCustomersData = new();
        public static List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> FTCXeroCustomersData = new();
        public static PortlandXeroLib.XeroConnector _xeroconnector;
        private readonly ILogger<HomeController> _logger;
        private readonly IQueriesRepository _db;
        public HomeController(ILogger<HomeController> logger, IQueriesRepository db)

        {
            _logger = logger;
            _db = db;
        }
        [HttpPost]
        public IActionResult CustomerDetails([FromBody]string? xeroID)
        {
            CustomerDetailsModels pageModel = new();
            pageModel.CustomerLists = RetrieveCustomer.CustomerDetailsLoadData();
            if(string.IsNullOrEmpty(xeroID) == false)
            {
                pageModel.CustomerModel = RetrieveCustomer.GetCustomerInformation(xeroID);
            }
            else
            {
                pageModel.CustomerModel = new();
            }
            ViewData["Title"] = "Customer Details";
            return View("/Views/CustomerDetails/CustomerDetails.cshtml", pageModel);
        }

        public ActionResult Edi()
        {
            ViewData["Title"] = "Edi's";
            var model = LoadEdiVmModel();
            return View("/Views/Edi/Edi.cshtml",model);
        }
        public static EDIVM LoadEdiVmModel()
        {

           EDIVM model = new();
            model.EDIs = new();
            FuelcardsContext _fuelcardrepo = new();
            IEnumerable<FcControl> edis = _fuelcardrepo.FcControls.Where(e => e.Invoiced != true);
            foreach (var item in edis)
            {
                if (item.Network != 0) item.TotalQuantity = item.TotalQuantity / 100;
                FcControlVM _fcControl = FcControlVM.Map(item);
                _fcControl.DayOfTheWeek = item.CreationDate.Value.DayOfWeek;
                model.EDIs.Add(_fcControl);
            }
            model.EDIs = model.EDIs.OrderByDescending(e => e.CreationDate).ToList();
            return model;
        }
        public ActionResult Invoicing()
        {
            ViewData["Title"] = "Invoicing";
            return View();
        }
        public async Task<IActionResult> Homepage()
        {
            if (PFLXeroCustomersData.Count == 0)
            {
                await ConnectingToXero.GetFuelcardCustomers(PFLXeroCustomersData, FTCXeroCustomersData, _xeroconnector);
            }
            return View();
        }
        public async Task<IActionResult> Index()
        {
            if (PFLXeroCustomersData.Count != 0)
            {
                await ConnectingToXero.GetFuelcardCustomers(PFLXeroCustomersData, FTCXeroCustomersData, _xeroconnector);
            }


            ViewData["Title"] = "Home";
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetLink()
        {
            HttpClient client = new();
            var xero = new XeroConnector(client);
            _xeroconnector = xero;
            var AuthLink = await xero.GetLinkAndOpen();
            return Json(AuthLink);
        }

        [HttpPost]
        public JsonResult CheckXeroConnection()
        {
            if (PFLXeroCustomersData.Count != 0)
            {
                return Json("Already connected to xero");
            }
            else return Json("Not connected to xero");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
