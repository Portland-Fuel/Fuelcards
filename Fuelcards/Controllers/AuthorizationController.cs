using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PortlandXeroLib;
using System;
using System.Threading.Tasks;

namespace Fuelcards.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Callback(string code, string state)
        {
            var authCodes = new PortlandXeroLib.ReturnedAuthCodes();
            authCodes.AuthorisationCode = code;
            authCodes.State = state;

            XeroConnector._authCodes = authCodes;
            return RedirectToAction("Homepage", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}