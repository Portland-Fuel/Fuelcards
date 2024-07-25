using DataAccess.Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;
using Microsoft.Graph;
using static Fuelcards.Controllers.InvoicingController;

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
       


        internal void processTransaction(InvoicingController.TransactionDataFromView transactionDataFromView, EnumHelper.Network network)
        {
            EnumHelper.Products? product = EnumHelper.GetProductFromProductCode(Convert.ToInt32(transactionDataFromView.transaction.ProductCode), network);
            Models.Site? siteInfo = getSite(transactionDataFromView.transaction.SiteCode, network);
            double? Addon = _db.GetAddonForSpecificTransaction(transactionDataFromView.transaction.PortlandId,transactionDataFromView.transaction.TransactionDate,network,transactionDataFromView.IfuelsCustomer);
            if(transactionDataFromView.customerType == EnumHelper.CustomerType.Fix)
            {
                var stuff = "Do Stuff";
            }
            if(transactionDataFromView.IfuelsCustomer == true)
            {
                var egg = "Cluckles";
            }

        }

        private double GetSiteSurcharge(int? siteCode, EnumHelper.Network network)
        {
            throw new NotImplementedException();
        }

        private Models.Site? getSite(int? siteCode, EnumHelper.Network network)
        {
            if (siteCode == null) throw new ArgumentNullException("The site code should not be null. now that it is - This needs to be coded...");
            SiteNumberToBand? site = _sites.Where(e => e.SiteNumber == siteCode && e.NetworkId == (int)network && e.Active != false).FirstOrDefault();

            Models.Site foundSite = new()
            {
                name = site.Name,
                band = site.Band,
            };
            foundSite.Surcharge = _db.GetSurchargeFromBand(foundSite.band, network);

            if (site == null && network == EnumHelper.Network.Keyfuels) { foundSite.name = "EMAIL PIN CHARGE"; }
            return foundSite;
        }
       
    }
}
