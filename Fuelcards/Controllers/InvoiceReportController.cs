using DataAccess.Fuelcards;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fuelcards.Controllers
{

    public class InvoiceReportController : Controller
    {
        private readonly IQueriesRepository _db;

        public InvoiceReportController(IQueriesRepository db)
        {
            _db = db;
        }
        [HttpPost]
        public JsonResult getIT([FromBody] string date)
        {
            try
            {
                DateOnly topass = DateOnly.FromDateTime(DateTime.Parse(date));
                var Rep =  _db.getInvoiceReport(topass);
                Rep.Add(CalculateTotals(Rep));
                return Json(Rep);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static InvoiceReport CalculateTotals(List<InvoiceReport> reportList)
        {
            InvoiceReport totalsRow = new InvoiceReport();

            // Get all properties of the InvoiceReport class that are nullable decimals
            var properties = typeof(InvoiceReport).GetProperties()
                .Where(p => p.PropertyType == typeof(double?));

            foreach (var property in properties)
            {
                // Sum the values for each property
                double? sum = reportList.Sum(r => (double?)property.GetValue(r) ?? 0);

                // Round the result to 2 decimal places
                sum = Math.Round(sum.Value, 2);

                // Set the summed and rounded value to the corresponding property of totalsRow
                property.SetValue(totalsRow, sum);
            }

            return totalsRow;
        }

    }

}
