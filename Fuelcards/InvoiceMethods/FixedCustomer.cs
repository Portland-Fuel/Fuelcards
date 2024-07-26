using DataAccess.Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.Graph;
using Xero.NetStandard.OAuth2.Model.Accounting;

namespace Fuelcards.InvoiceMethods
{
    public class FixedCustomer
    {
        public static int? account = null;
        public static double? FixedVolumeUsedOnThisInvoice = null;
        public static double? RolledVolumeUsedOnThisInvoice = null;
        public static double? AvailableRolledVolume = null;
        public static double? FixedPrice = null;
        internal double? CalculateFixTransactionPrice(InvoicingController.TransactionDataFromView data, Models.Site site, EnumHelper.Network network, IQueriesRepository db)
        {
            CheckIfStaticVariablesNeedUpdating((int)data.account, data.fixedInformation.RolledVolume, data.fixedInformation.AllFixes.Where(e => e.Id == data.fixedInformation.CurrentTradeId).FirstOrDefault().FixedPriceIncDuty);

            double? QuantityToBePriced = data.transaction.Quantity;
            double? rolled = AvailableRolledVolume;
            if (rolled is not null && rolled > 0)
            {
                rolled = rolled - QuantityToBePriced;
                rolled = Convert.ToDouble(Math.Round(Convert.ToDecimal(rolled), 2));
                if (rolled > 0)
                {
                    AvailableRolledVolume = rolled;
                    RolledVolumeUsedOnThisInvoice += QuantityToBePriced;
                    FixedVolumeUsedOnThisInvoice += QuantityToBePriced;
                    return 0;
                }
                else if (rolled < 0)
                {
                    double? VolumeToCharge = Convert.ToDouble(Math.Abs(Convert.ToDecimal(rolled)));
                    FixedVolumeUsedOnThisInvoice += QuantityToBePriced;
                    double? price = VolumeToCharge * (FixedPrice/100);
                    return price / QuantityToBePriced;
                }
            }

            return 0;
        }

        private void CheckIfStaticVariablesNeedUpdating(int? currentAccount, double? StartingRoll, double? newFixPrice)
        {
            if (account is null || currentAccount != account)
            {
                account = currentAccount;
                FixedVolumeUsedOnThisInvoice = 0;
                RolledVolumeUsedOnThisInvoice = 0;
                AvailableRolledVolume = StartingRoll;
                FixedPrice = newFixPrice;
            }
        }
    }
}
