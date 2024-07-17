using Fuelcards.Repositories;

namespace Fuelcards.GenericClassFiles
{
    public class InvoiceChecks
    {
        private readonly IQueriesRepository _db;
        public InvoiceChecks(IQueriesRepository db)
        {
            _db = db;
        }
        
    }
}
