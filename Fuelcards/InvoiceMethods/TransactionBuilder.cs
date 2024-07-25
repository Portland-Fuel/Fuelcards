using DataAccess.Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;
using Microsoft.Graph;

namespace Fuelcards.InvoiceMethods
{
    public class TransactionBuilder
    {
        private readonly List<SiteNumberToBand> _sites;
        private readonly IQueriesRepository _db;

        public TransactionBuilder(List<SiteNumberToBand> sites, IQueriesRepository db)
        {
            _sites = sites;
            _db = db;
        }
        public string getSiteName(int? siteCode, EnumHelper.Network network)
        {
            if (siteCode == null && network == EnumHelper.Network.Keyfuels) { return "EMAIL PIN CHARGE"; }
            if (siteCode == null) throw new ArgumentNullException("The site code should not be null. now that it is - This needs to be coded...");
            string? name = _sites.Where(e => e.SiteNumber == siteCode && e.NetworkId == (int)network && e.Active != false).FirstOrDefault()?.Name;
            return name;
        }


        internal void processTransaction(InvoicingController.TransactionDataFromView transactionDataFromView, EnumHelper.Network network)
        {

            EnumHelper.Products? product = EnumHelper.GetProductFromProductCode(Convert.ToInt32(transactionDataFromView.transaction.ProductCode), network);
            


        }

    }
}
