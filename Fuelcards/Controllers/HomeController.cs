
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
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

        public HomeController(ILogger<HomeController> logger)

        {
            _logger = logger;
        }
        public ActionResult CustomerDetails()
        {
            ViewData["Title"] = "Customer Details";
            return View("/Views/CustomerDetails/CustomerDetails.cshtml");
        }

        public ActionResult Edi()
        {
            ViewData["Title"] = "Edi's";
            return View();
        }

        public ActionResult Invoicing()
        {
            ViewData["Title"] = "Invoicing";
            return View();
        }
        public IActionResult Homepage()
        {
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
