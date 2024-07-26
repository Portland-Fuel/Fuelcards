using DataAccess.Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;
using Microsoft.Graph;
using static Fuelcards.Controllers.InvoicingController;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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



        internal DataToPassBack processTransaction(InvoicingController.TransactionDataFromView transactionDataFromView, EnumHelper.Network network)
        {
            EnumHelper.Products? product = EnumHelper.GetProductFromProductCode(Convert.ToInt32(transactionDataFromView.transaction.ProductCode), network);
            Models.Site? siteInfo = getSite(transactionDataFromView.transaction.SiteCode, network);
            transactionDataFromView.transaction.Quantity = ConvertToLitresBasedOnNetwork(transactionDataFromView.transaction.Quantity, network);
            double? Addon = _db.GetAddonForSpecificTransaction(transactionDataFromView.transaction.PortlandId, transactionDataFromView.transaction.TransactionDate, network, transactionDataFromView.IfuelsCustomer, (int)transactionDataFromView.account);

            double? UnitPrice = Math.Round(Convert.ToDouble(CalculateUnitPrice(_db, product, transactionDataFromView, network, Addon, siteInfo)), 4, MidpointRounding.AwayFromZero);

            {
                var stuff = "CODE THIS SOON";
            }
            double? invoicePrice = transactionDataFromView.transaction.Quantity * UnitPrice;
            invoicePrice = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoicePrice), 2, MidpointRounding.AwayFromZero));
            DataToPassBack model = new()
            {
                Product = product.ToString(),
                SiteName = siteInfo.name,
                UnitPrice = UnitPrice.HasValue ? UnitPrice.Value.ToString("F4") : null,
                InvoicePrice = invoicePrice.HasValue ? invoicePrice.Value.ToString("F2") : null
            };
            return model;

        }

        private double? CalculateUnitPrice(IQueriesRepository _db,EnumHelper.Products? product,TransactionDataFromView data, EnumHelper.Network network, double? Addon, Models.Site site)
        {
            try
            {
                ProductCalculations methodology = new();
                switch (product)
                {
                    case EnumHelper.Products.Diesel: return methodology.Diesel(data, Addon, site, network, _db, (EnumHelper.Products)product);
                    case EnumHelper.Products.Adblue: return methodology.AdblueMethodology(data.transaction, network);
                    case EnumHelper.Products.TescoDieselNewDiesel: return methodology.TescoNewDiesel(data.transaction, network);
                    case EnumHelper.Products.ULSP: return methodology.ULSP(data.transaction, network);
                    case EnumHelper.Products.Lube: return methodology.Lube(data.transaction, network);
                    case EnumHelper.Products.SuperUnleaded: return methodology.SuperUnleaded(data.transaction, network);
                    case EnumHelper.Products.PremiumDiesel: return methodology.PremiumDiesel(data.transaction, network);
                    case EnumHelper.Products.Tolls: return methodology.Tolls(data.transaction, network);
                    case EnumHelper.Products.Other: return methodology.Other(data.transaction, network);
                    case EnumHelper.Products.AdblueCan: return methodology.AdblueCan(data.transaction, network);
                    case EnumHelper.Products.PackagedAdblue: return methodology.PackagedAdblue(data.transaction, network, _db);
                    case EnumHelper.Products.LPG: return methodology.LPG(data.transaction, network);
                    case EnumHelper.Products.Goods: return methodology.Goods(data.transaction, network);
                    case EnumHelper.Products.HVO: return methodology.HVO(data.transaction, network);
                    case EnumHelper.Products.Card: return methodology.Card(data.transaction, network);
                    case EnumHelper.Products.CardStopManagementFee: return 0;
                    case EnumHelper.Products.EDISTDSignle: return 0;
                    case EnumHelper.Products.StockNotification: return 0;
                }
                return null;







            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to produce a floating price");
            }

        }
        private Models.Site? getSite(int? siteCode, EnumHelper.Network network)
        {
            if (siteCode == null) throw new ArgumentNullException("The site code should not be null. now that it is - This needs to be coded...");
            SiteNumberToBand? site = _sites.Where(e => e.SiteNumber == siteCode && e.NetworkId == (int)network && e.Active != false).FirstOrDefault();

            Models.Site foundSite = new()
            {
                name = site.Name,
                band = site.Band,
                code = (int)site.SiteNumber,
            };
            foundSite.Surcharge = _db.GetSurchargeFromBand(foundSite.band, network);
            if (site == null && network == EnumHelper.Network.Keyfuels) { foundSite.name = "EMAIL PIN CHARGE"; }
            return foundSite;
        }
        public static double? ConvertToLitresBasedOnNetwork(double? quantity, EnumHelper.Network network)
        {
            switch (network)
            {
                case EnumHelper.Network.UkFuel:
                    return quantity / 100;
                case EnumHelper.Network.Keyfuels:
                    return quantity;
                case EnumHelper.Network.Texaco:
                    return quantity / 100;
            }
            return quantity;
        }
    }
}
