using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.Graph;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using static Fuelcards.GenericClassFiles.EnumHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fuelcards.InvoiceMethods
{
    public class ProductCalculations
    {
        //public double? Diesel(InvoicingController.TransactionDataFromView data, double? addon, Models.Site site, EnumHelper.Network network, IQueriesRepository _db,EnumHelper.Products product)
        //{
        //    DieselTransaction diesel = new();
            
            
        //    //if(data.customerType == CustomerType.Fix)
        //    //{
        //    //    FixedCustomer processFix = new();
        //    //    return processFix.CalculateFixTransactionPrice(data, site, network, _db,addon,product);
        //    //}
        //    //else
        //    //{
        //    //    return DieselFloatingUnitPrice(_db,product,data,network,site,addon);
        //    //}
        //}
        public double? AdblueMethodology(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                switch (network)
                {
                    case EnumHelper.Network.Keyfuels:
                        double? NewValue = 1.6 * (data.cost / 1.1) / 100;
                        //return NewValue;
                        double? price =  NewValue * 100;
                        return price / data.quantity;
                    case EnumHelper.Network.UkFuel:

                        double? NewValue2 = 1.45 * (data.cost / 100);
                        return NewValue2;
                    //return NewValue2 * 100;
                    case EnumHelper.Network.Texaco:

                        double? NewValue3 = 1.6 * (data.cost / 100);
                        //return NewValue3 * 100;
                        return NewValue3;
                    case EnumHelper.Network.Fuelgenie:
                        return data.cost;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? TescoNewDiesel(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                if (network == EnumHelper.Network.Keyfuels)
                {
                    double? KeyValue = 1.03 * (data.cost / 1.01) / 100;
                    var price = KeyValue * 100;
                    double? unitPrice = price / data.quantity;
                    return unitPrice;
                }
                double? NewValue = 1.02 * (data.cost / 1.01) / 100;
                return NewValue * 100;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? ULSP(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                switch (network)
                {
                    case EnumHelper.Network.Keyfuels:
                        double? SpecificValue = 1.15 * (data.cost / 1.05) / 100;
                        var price = SpecificValue * 100;
                        double? UnitPrice = price / data.quantity;
                        return UnitPrice;

                    case EnumHelper.Network.UkFuel:
                        double? NewValueU = 1.14 * (data.cost / 1.06) / 100;
                        //var priceU = NewValueU * 100;
                        return NewValueU;
                    case EnumHelper.Network.Texaco:
                        double? NewValue3 = (1.1 * data.cost) / 100;
                        //var priceU3 = NewValue3 * 100;
                        return NewValue3;
                    case EnumHelper.Network.Fuelgenie:
                        return data.cost;

                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? ConvertToLitresBasedOnNetwork(double? quantity, EnumHelper.Network network)
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
        public double? Lube(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                switch (network)
                {
                    case EnumHelper.Network.Keyfuels:
                        double? NewValue = 1.35 * (data.cost / 1.15) / 100;
                        double? price =  NewValue * 100;
                        return price / data.quantity;

                    case EnumHelper.Network.UkFuel:
                        //double? NewValue2 = 1.2 * (data.cost / 1.1) / 100; OLD
                        double? NewValue2 = 1.4 * (data.cost / 1.1) / 100;
                        return NewValue2;
                    case EnumHelper.Network.Texaco:
                        double? NewValue3 = 1.2 * (data.cost) / 100;
                        //return NewValue3 * 100;
                        return NewValue3;
                    case EnumHelper.Network.Fuelgenie:
                        return data.cost;
                    default: return null;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? SuperUnleaded(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                switch (network)
                {
                    case EnumHelper.Network.Keyfuels:
                        double? NewValue = 1.35 * (data.cost / 1.15) / 100;
                        var price = NewValue * 100;
                        return price / data.quantity;
                    case EnumHelper.Network.UkFuel:
                        double? NewValue2 = 1.2 * (data.cost / 1.05) / 100;
                        //return NewValue2 * 100;
                        return NewValue2;
                    case EnumHelper.Network.Texaco:
                        if (data.cost > 500) data.cost = data.cost / 100;
                        double? NewValue3 = 1.2 * (data.cost) / 100;
                        return NewValue3 * 100;
                    case EnumHelper.Network.Fuelgenie:
                        return data.cost;
                    default: return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? Other(GenericTransactionFile data, EnumHelper.Network network)
        {
            if (network == EnumHelper.Network.Fuelgenie) return data.cost;
            if (network == EnumHelper.Network.UkFuel)
            {
                double? NewValue = 1.2 * (data.cost / 1.15) / 100;
                var a = (NewValue * 100) / data.quantity;
                return NewValue * 100;
            }
            try
            {
                double? NewValue = 1.2 * (data.cost / 1.1) / 100;
                var a = (NewValue * 100) / data.quantity;
                return NewValue * 100;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? PremiumDiesel(GenericTransactionFile data, EnumHelper.Network network)
        {
            switch (network)
            {
                case EnumHelper.Network.UkFuel:
                    double? NewValue = 1.2 * (data.cost / 1.05) / 100;
                    //return NewValue * 100;
                    return NewValue;
                case EnumHelper.Network.Keyfuels:
                    double? NewValue2 = 1.35 * (data.cost / 1.15) / 100;
                    double? price = NewValue2 * 100;
                    return price / data.quantity;

                case EnumHelper.Network.Texaco:
                    double? NewValue3 = (1.2 * data.cost) / 100;
                    //return NewValue3 * 100;
                    return NewValue3;
                case EnumHelper.Network.Fuelgenie:
                    return data.cost;
                default: return null;
            }
        }
        public double? AdblueCan(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                switch (network)
                {
                    case EnumHelper.Network.Keyfuels:
                        double? NewValue = 1.6 * (data.cost / 1.1) / 100;
                        var price = NewValue * 100;
                        return price / data.quantity;
                    case EnumHelper.Network.UkFuel:

                        double? NewValue2 = 1.6 * (data.cost / 1.15 / 100);
                        return NewValue2;
                    case EnumHelper.Network.Texaco:

                        double? NewValue3 = (1.6 * data.cost) / 100;
                        return NewValue3;
                    case EnumHelper.Network.Fuelgenie:
                        return data.cost;
                    default: return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? Tolls(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {
                double? NewValue = 1.2 * (data.cost / 1.05) / 100;

                var price = NewValue * 100;
                return price / data.quantity;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? Card(GenericTransactionFile data, EnumHelper.Network network)
        {
            if (network != EnumHelper.Network.Keyfuels) throw new ArgumentException("So far only keyfuels transactions should be able to enter the calculation for a new card! something has gone badly wrong");
            double? NewValue = 1.25 * (data.cost / 1.1) / 100;
            var price =  NewValue * 100;
            return price/data.quantity;
        }
        public double? PackagedAdblue(GenericTransactionFile data, EnumHelper.Network network, IQueriesRepository _db)
        {
            try
            {
                if (data.cost == 0 && data.productCode == 18 && network == EnumHelper.Network.UkFuel)
                {

                    data.cost = _db.GetMissingProduct(network, data.productCode);
                    double? NewValue = 1.75 * (data.cost / 1.2) / 100;
                    return NewValue * 100;
                }
                if (data.cost > 0 && data.productCode == 18)
                {
                    double? NewValue = 1.75 * data.cost / 100;
                    return NewValue * 100;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? LPG(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {

                double? NewValue = 1.2 * data.cost / 100;

                return NewValue * 100;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? Goods(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {

                double? NewValue = 1.2 * (data.cost / 100) / 1.05;

                return NewValue * 100;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? HVO(GenericTransactionFile data, EnumHelper.Network network)
        {
            try
            {

                double? NewValue = 1.2 * (data.cost / 100);

                return NewValue * 100;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double? DieselFloatingUnitPrice(IQueriesRepository _db, EnumHelper.Products product,InvoicingController.TransactionDataFromView data, EnumHelper.Network network, Models.Site site, double? Addon)
        {
            if (product == EnumHelper.Products.Diesel)
            {
                double? basePrice = _db.GetBasePrice((DateOnly)data.transaction.transactionDate);
                double? transactionSite = site.transactionalSiteSurcharge;
                double? UnitPrice = basePrice + Addon + transactionSite + site.Surcharge;
                UnitPrice = UnitPrice / 100;
                return UnitPrice;
            }
            return 0;
        }

        
    }
}
