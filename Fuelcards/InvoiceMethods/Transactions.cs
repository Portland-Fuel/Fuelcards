using DataAccess.Fuelcards;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Xero.NetStandard.OAuth2.Client;

namespace Fuelcards.InvoiceMethods
{
    public class Transactions
    {
        private readonly IQueriesRepository _db;
        public Transactions(IQueriesRepository db)
        {
            _db = db;
        }
        
        private static DateOnly GetMostRecentMonday(DateOnly currentDate, string network)
        {
            if (network == "Fuelgenie") return currentDate;
            int daysUntilMonday = ((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateOnly mostRecentMonday = currentDate.AddDays(-daysUntilMonday);
            return mostRecentMonday;
        }
    }
}
