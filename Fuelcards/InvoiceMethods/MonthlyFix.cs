
using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;

namespace Fuelcards.InvoiceMethods
{
    public class MonthlyFix
    {
        //public static int TradeId { get; set; }
        //public static double? Rolled { get; set; }
        //public static double Current { get; set; }
        //public static double NewRolloverCurrent { get; set; }


        internal static bool CheckIfRolloverWeek(DateOnly invoiceDate)
        {
            var startDate = invoiceDate.AddDays(-6);
            var endDate = invoiceDate;

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.Month != startDate.Month)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

//        internal static void ProcessVolumes(InvoicingController.TransactionDataFromView data, IQueriesRepository _db)
//        {
//            bool isRolloverWeek = CheckIfRolloverWeek(data.invoiceDate);
//            if (isRolloverWeek) 
//            {
//                double? FixedVolumeRemainingForCurrent = _db.GetRemaingVolumeForCurrentAllocation(_db.GetAllocationAtTimeOfTransaction(data.transaction.transactionDate, data.fixedInformation.CurrentTradeId));
//            }
//        }
//        private static async Task ProcessRolloverVolumes(InvoicingController.TransactionDataFromView data, EnumHelper.Network network, Models.Site siteInfo)
//        {

//            double QuantityRemainingToBeAllocated = (double)data.transaction.quantity;
//            if (network == EnumHelper.Network.UkFuel && (siteInfo.band == "9" || siteInfo.band == "8"))
//            {
//                VolumeChargedAtFloating = QuantityRemainingToBeAllocated;
//                TotalDieselUsed += QuantityRemainingToBeAllocated;
//                return;
//            }
//            while (QuantityRemainingToBeAllocated > 0 && (FixedVolumeRemainingForCurrent > 0 || FixedVolumeRemainingForCurrent is null))
//            {

//                var originalVolume = AvailableRolledVolume;
//                var newVolume = UpdateVolume(originalVolume, ref QuantityRemainingToBeAllocated);

//                if (newVolume < 0)
//                {
//                    QuantityRemainingToBeAllocated = (double)HandleNegativeVolume(newVolume);
//                }
//            }

//            if (QuantityRemainingToBeAllocated > 0)
//            {
//                VolumeChargedAtFloating = QuantityRemainingToBeAllocated;
//            }
//            return;
//        }
//        public void UpdateModel()
//        {

//        }

//    }
//}
