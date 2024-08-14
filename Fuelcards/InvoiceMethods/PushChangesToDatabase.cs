
using Fuelcards.Repositories;
using Microsoft.Graph;

namespace Fuelcards.InvoiceMethods
{
    public class PushChangesToDatabase
    {
        internal static void SubmitFinalTransactionToDatabase(InvoicePDFModel? invoice, IQueriesRepository _db)
        {
            foreach (var transaction in invoice.transactions)
            {
                var Transaction = _db.GetTransaction(transaction.TransactionNumber, invoice.network);
            }
        }
    }
}
