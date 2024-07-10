using Fuelcards.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Fuelcards.Controllers
{
    public class HomeController : Controller
    {
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
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
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
