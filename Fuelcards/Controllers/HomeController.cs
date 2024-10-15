
using DataAccess.Fuelcards;
using FuelcardModels.DataTypes;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult InvoiceReport()
        {
            InvoiceReportVM model = new InvoiceReportVM();

            model.dates = _db.GetInvoiceReportDates();

            model.dates = model.dates
                .OrderByDescending(e => e)  
                .Distinct()                
                .ToList();

            ViewData["Title"] = "Invoice Report";

            return View("/Views/InvoiceReport/InvoiceReport.cshtml", model);
        }


        [HttpPost]
        public IActionResult CustomerDetails()
        {
            RetrieveCustomer customerClass = new(_db);
            CustomerDetailsModels pageModel = new();
            pageModel.CustomerLists = customerClass.CustomerDetailsLoadData();

            ViewData["Title"] = "Customer Details";
            return View("/Views/CustomerDetails/NewCustomerDetails.cshtml", pageModel);
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


            model.networkselectListItems = new();

            List<EnumHelper.Network> AllNetworks = new();

            AllNetworks.Add(EnumHelper.Network.Keyfuels);
            AllNetworks.Add(EnumHelper.Network.UkFuel);
            AllNetworks.Add(EnumHelper.Network.Fuelgenie);
            AllNetworks.Add(EnumHelper.Network.Texaco);

            foreach (var item in AllNetworks)
            {
                SelectListItem selectListItem = new();
                selectListItem.Value = item.ToString();
                selectListItem.Text = EnumHelper.NetworkEnumFromInt(Convert.ToInt16(item)).ToString();
                model.networkselectListItems.Add(selectListItem);
            }




            return model;
        }
        public ActionResult Invoicing()
        {
            ViewData["Title"] = "Invoicing";

            return RedirectToAction("Invoicing", "Invoicing");
        }
        public async Task<IActionResult> Homepage()
        {
            if (PFLXeroCustomersData.Count == 0)
            {
                await ConnectingToXero.GetFuelcardCustomers(PFLXeroCustomersData, FTCXeroCustomersData, _xeroconnector);
                InvoicingController.reportList = new();
                InvoicingController.invoices = new();
            }
            return View();
        }
        public IActionResult Index()
        {
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
