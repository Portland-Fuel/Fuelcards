using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Fuelcards.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IQueriesRepository _db;
        public TransactionsController(IQueriesRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult ReturnTransaction([FromBody] TransactionLookup data)
        {
            List<GenericTransactionFile> genericTransaction = new();
            switch (data.Network)
            {
                case EnumHelper.Network.Keyfuels:
                    genericTransaction = KeyfuelsTransactions(data, genericTransaction);
                    break;
                case EnumHelper.Network.UkFuel:
                    genericTransaction = UkFuelTransactions(data, genericTransaction);
                    break;
                case EnumHelper.Network.Texaco:
                    genericTransaction = TexacoTransactions(data, genericTransaction);
                    break;
            }
            return Json(genericTransaction);
        }
        public List<GenericTransactionFile> TexacoTransactions(TransactionLookup data, List<GenericTransactionFile> transactions)
        {
            var transaction = _db.RequiredTexacoTransactions(data);
            return transactions;
        }
        public List<GenericTransactionFile> UkFuelTransactions(TransactionLookup data, List<GenericTransactionFile> transactions)
        {
            var transaction = _db.RequiredUkFuelTransactions(data);

            return transactions;
        }
        public List<GenericTransactionFile> KeyfuelsTransactions(TransactionLookup data, List<GenericTransactionFile> transactions)
        {
            var transaction = _db.RequiredKeyfuelTransactions(data);

            return transactions;
        }
    }
    public class TransactionLookup
    {
        public EnumHelper.Network Network { get; set; }
        public decimal? TransactionNumber { get; set; }
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public int? account { get; set; }
    }
}
