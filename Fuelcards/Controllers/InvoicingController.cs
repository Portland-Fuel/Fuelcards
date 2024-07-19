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
            InvoicePreCheckModels _checks = new(_db);
            _checks.texacoVolume = new(_db);
            var egg = _checks.keyfuelsInvoiceList;
            return View("/Views/Invoicing/RealInvoicing.cshtml", _checks);
        }

    }
}
