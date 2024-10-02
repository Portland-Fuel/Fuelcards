using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Fuelcards.Controllers
{
    [Route("TransactionsController/[action]")]
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
        public struct DeleteTransactionsRequest
        {
            public List<int> TransactionNumbers { get; set; }

            public string Network { get; set; }
        }

        [HttpPost]
        public IActionResult DeleteTransactions([FromBody] DeleteTransactionsRequest request)
        {

            var EnumOfNetwork = EnumHelper.NetworkEnumFromString(request.Network);
            try
            {
                if (request.TransactionNumbers != null && request.TransactionNumbers.Any())
                {
                    foreach (var transactionNumber in request.TransactionNumbers)
                    {
                        switch (EnumOfNetwork)
                        {
                            case EnumHelper.Network.Keyfuels:
                                _db.DeleteKeyfuelTransaction(transactionNumber);
                                break;
                            case EnumHelper.Network.UkFuel:
                                _db.DeleteUkFuelTransaction(transactionNumber);
                                break;
                            case EnumHelper.Network.Texaco:
                                _db.DeleteTexacoTransaction(transactionNumber);
                                break;
                            case EnumHelper.Network.Fuelgenie:
                                _db.DeleteFuelgenieTransaction(transactionNumber);
                                break;
                        }
                    }

                    return Json(new { success = true, message = "Transactions deleted successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "No transactions selected for deletion." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting transactions.", error = ex.Message });
            }
        }





        [HttpPost]
        public JsonResult ReturnTransaction([FromBody] TransactionLookup TransactionLookup)
        {
            try
            {
                List<GenericTransactionFile> genericTransaction = new();

                var EnumNetwork = EnumHelper.NetworkEnumFromString(TransactionLookup.Network);
                switch (EnumNetwork)
                {
                    case EnumHelper.Network.Keyfuels:
                        genericTransaction = KeyfuelsTransactions(TransactionLookup, genericTransaction);
                        break;
                    case EnumHelper.Network.UkFuel:
                        genericTransaction = UkFuelTransactions(TransactionLookup, genericTransaction);
                        break;
                    case EnumHelper.Network.Texaco:
                        genericTransaction = TexacoTransactions(TransactionLookup, genericTransaction);
                        break;
                }
                return Json(genericTransaction);
            } 
            catch (Exception e)
            {

                throw;
            }
           
        }
        public List<GenericTransactionFile> TexacoTransactions(TransactionLookup data, List<GenericTransactionFile> transactions)
        {
            var transaction = _db.RequiredTexacoTransactions(data);
            return transaction.Result;
        }
        public List<GenericTransactionFile> UkFuelTransactions(TransactionLookup data, List<GenericTransactionFile> transactions)
        {
            var transaction = _db.RequiredUkFuelTransactions(data);

            return transaction.Result;
        }
        public List<GenericTransactionFile> KeyfuelsTransactions(TransactionLookup data, List<GenericTransactionFile> transactions)
        {
            var transaction = _db.RequiredKeyfuelTransactions(data);

            return transaction.Result;
        }
    }
    public class TransactionLookup
    {
        public string? Network { get; set; }
        public string? TransactionNumber { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? account { get; set; }
    }



    public class Trans
    {
        public string? Network { get; set; }
        public string? TransactionNumber { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? account { get; set; }
    }



}
