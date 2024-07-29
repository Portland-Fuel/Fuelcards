using System;
using Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.GenericClassFiles;
using Fuelcards.Repositories;
using Microsoft.Graph;
namespace Fuelcards.InvoiceMethods
{
    public class DieselTransaction
    {
        public static double? VolumeChargedAtFix = 0;
        public static double? VolumeChargedAtFloating = 0;
        //public static double? PriceToInvoice = 0;
        //public static List<double[]> AllocationIdsUsedForThisTransaction = new();
        //public static List<double[]> AllocationIdsUsedForInvoice = new();

        //public static int? CurrentAllocationId = null;

        //public static double? TotalDieselUsedOnThisInvoice = 0;
        //public static double? TotalVolumeRolled = 0;
        //public static double TotalFixedVolumeUsedOnThisInvoice = 0;
        //public static double? VolumeTakenFromTheCurrentAllocation = 0;



        public static int? account = null;
        public static double? FixedVolumeUsedOnThisInvoice = null;
        public static double? RolledVolumeUsedOnThisInvoice = null;
        public static double? AvailableRolledVolume = null;
        public static double? FixedPrice = null;
        public static double? Price2 = null;  // Assuming a secondary price level is defined.
        public static double? FixedVolumeRemainingForCurrent = null;
        public static double? FixRate { get; set; }
        public static double? FloatingRate { get; set; }
        private static void CheckIfStaticVariablesNeedUpdating(int? currentAccount, double? StartingRoll, double? newFixPrice, double? FixedVolumeCurrent)
        {
            if (account is null || currentAccount != account)
            {
                account = currentAccount;
                FixedVolumeUsedOnThisInvoice = 0;
                RolledVolumeUsedOnThisInvoice = 0;
                AvailableRolledVolume = StartingRoll;
                FixedPrice = newFixPrice;
                FixedVolumeRemainingForCurrent = FixedVolumeCurrent;
            }
        }
        public static double? DieselMethodology(InvoicingController.TransactionDataFromView data, double? addon, Models.Site SiteInfo, EnumHelper.Network network, IQueriesRepository _db, EnumHelper.Products product)
        {
            
            try
            {
                GetFixAndFloatingRate(FixedPrice, SiteInfo, addon, _db.GetBasePrice((DateOnly)data.transaction.TransactionDate),data.customerType);

                if (data.customerType == GenericClassFiles.EnumHelper.CustomerType.Floating)
                {
                    VolumeChargedAtFloating = data.transaction.Quantity;
                }
                else
                {
                    CheckIfStaticVariablesNeedUpdating((int)data.account, data.fixedInformation.RolledVolume, data.fixedInformation.AllFixes.Where(e => e.Id == data.fixedInformation.CurrentTradeId).FirstOrDefault().FixedPriceIncDuty, data.fixedInformation.AllFixes.Where(e => e.Id == data.fixedInformation.CurrentTradeId).FirstOrDefault().FixedVolume);
                    ProcessRolloverVolumes(data);
                }
              var e = CalculatePrice(data, network);
                return e;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetFixAndFloatingRate(double? fixedPrice, Models.Site siteInfo, double? addon, double? basePrice, EnumHelper.CustomerType custType)
        {
            VolumeChargedAtFix = 0;
            VolumeChargedAtFloating = 0;
            FloatingRate = basePrice + addon + siteInfo.Surcharge + siteInfo.transactionalSiteSurcharge;
            if(custType == EnumHelper.CustomerType.Fix)
            {
                FixRate = fixedPrice + siteInfo.Surcharge;
            }
        }

        private static async Task ProcessRolloverVolumes(InvoicingController.TransactionDataFromView data)
        {
            double QuantityRemainingToBeAllocated = (double)data.transaction.Quantity;
            while (QuantityRemainingToBeAllocated > 0)
            {

                var originalVolume = AvailableRolledVolume;
                var newVolume = UpdateVolume(originalVolume, ref QuantityRemainingToBeAllocated);

                if (newVolume < 0)
                {
                    QuantityRemainingToBeAllocated = (double)HandleNegativeVolume(newVolume);
                }
            }

            if (QuantityRemainingToBeAllocated > 0)
            {
                VolumeChargedAtFloating = QuantityRemainingToBeAllocated;
            }
            return;
        }


        private static double? CalculatePrice(InvoicingController.TransactionDataFromView data, EnumHelper.Network network)
        {
            double? FixPrice = (VolumeChargedAtFix * FixRate) / 100;
            if (VolumeChargedAtFix == 0) FixPrice = 0;

            double? FloatingPrice = (VolumeChargedAtFloating * FloatingRate) / 100;

            if (network == EnumHelper.Network.Fuelgenie) FloatingPrice = data.transaction.Cost;
            if (FixPrice is null) FixPrice = 0;
            if (FloatingPrice is null) FloatingPrice = 0;
            return (FixPrice + FloatingPrice)/data.transaction.Quantity;
        }




        private static double? UpdateVolume(double? originalVolume, ref double QuantityRemainingToBeAllocated)
        {
            double? newVolume = originalVolume;
            newVolume = Convert.ToDouble(Math.Round(Convert.ToDecimal(originalVolume - QuantityRemainingToBeAllocated), 2));

            if (newVolume >= 0)
            {
                RolledVolumeUsedOnThisInvoice += QuantityRemainingToBeAllocated;
                FixedVolumeUsedOnThisInvoice += QuantityRemainingToBeAllocated;
                QuantityRemainingToBeAllocated = 0;
                AvailableRolledVolume = newVolume;
            }
            else
            {
                AvailableRolledVolume = 0;
            }
            return newVolume;
        }

        private static double? HandleNegativeVolume(double? newVolume)
        {
            newVolume = Math.Abs((double)newVolume);
            if (FixedVolumeRemainingForCurrent >= newVolume)
            {
                FixedVolumeRemainingForCurrent = FixedVolumeRemainingForCurrent - newVolume;
                FixedVolumeUsedOnThisInvoice += newVolume;
                VolumeChargedAtFix = newVolume;
                newVolume = 0;
                return newVolume;
            }
            else
            {
                VolumeChargedAtFix = FixedVolumeRemainingForCurrent;
                FixedVolumeUsedOnThisInvoice += FixedVolumeRemainingForCurrent;
                return newVolume - FixedVolumeRemainingForCurrent;
            }
        }
    }
}