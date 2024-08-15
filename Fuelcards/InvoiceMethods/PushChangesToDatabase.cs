
using DataAccess.Fuelcards;
using Fuelcards.Repositories;
using Microsoft.Graph;

namespace Fuelcards.InvoiceMethods
{
    public class PushChangesToDatabase
    {

        internal async static Task SubmitFinalTransactionToDatabase(InvoicePDFModel? invoice,IQueriesRepository _db)
        {
            foreach (var transaction in invoice.transactions)
            {
                await _db.UpdateDatabaseTransaction(transaction,invoice.CustomerDetails.InvoiceNumber, invoice.network);
            }
        }
    }
}
