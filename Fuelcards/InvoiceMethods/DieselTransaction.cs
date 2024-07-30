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

        public static double? TotalDieselUsed {  get; set; }
        public static int? account = null;
        public static double? FixedVolumeUsedOnThisInvoice = null;
        public static double? RolledVolumeUsedOnThisInvoice = null;
        public static double? AvailableRolledVolume = null;
        public static double? FixedPrice = null;
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
                TotalDieselUsed = 0;
            }
        }
        public static double? DieselMethodology(InvoicingController.TransactionDataFromView data, double? addon, Models.Site SiteInfo, EnumHelper.Network network, IQueriesRepository _db, EnumHelper.Products product)
        {
            
            try
            {
                GetFixAndFloatingRate(FixedPrice, SiteInfo, addon, _db.GetBasePrice((DateOnly)data.transaction.transactionDate),(EnumHelper.CustomerType)data.customerType);

                if (data.customerType == GenericClassFiles.EnumHelper.CustomerType.Floating)
                {
                    VolumeChargedAtFloating = data.transaction.quantity;
                }
                else
                {
                    CheckIfStaticVariablesNeedUpdating((int)data.account, data.fixedInformation.RolledVolume, data.fixedInformation.AllFixes.Where(e => e.Id == data.fixedInformation.CurrentTradeId).FirstOrDefault().FixedPriceIncDuty, data.fixedInformation.AllFixes.Where(e => e.Id == data.fixedInformation.CurrentTradeId).FirstOrDefault().FixedVolume);
                    ProcessRolloverVolumes(data);
                }
              var unitPrice = CalculatePrice(data, network);
                return unitPrice;
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
            double QuantityRemainingToBeAllocated = (double)data.transaction.quantity;
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
            FixedVolumeUsedOnThisInvoice += VolumeChargedAtFix;
            double? FloatingPrice = (VolumeChargedAtFloating * FloatingRate) / 100;

            if (network == EnumHelper.Network.Fuelgenie) FloatingPrice = data.transaction.cost;
            if (FixPrice is null) FixPrice = 0;
            if (FloatingPrice is null) FloatingPrice = 0;
            return (FixPrice + FloatingPrice)/data.transaction.quantity;
        }




        private static double? UpdateVolume(double? originalVolume, ref double QuantityRemainingToBeAllocated)
        {
            double? QuantityBeforeAlteration = QuantityRemainingToBeAllocated;
            double? newVolume = originalVolume;
            newVolume = Convert.ToDouble(Math.Round(Convert.ToDecimal(originalVolume - QuantityRemainingToBeAllocated), 2));

            if (newVolume >= 0)
            {
                FixedVolumeUsedOnThisInvoice += QuantityBeforeAlteration;
                RolledVolumeUsedOnThisInvoice += QuantityRemainingToBeAllocated;
                //FixedVolumeUsedOnThisInvoice += QuantityRemainingToBeAllocated;
                QuantityRemainingToBeAllocated = 0;
                AvailableRolledVolume = newVolume;
                //TotalDieselUsed += QuantityBeforeAlteration;
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
                //FixedVolumeUsedOnThisInvoice += newVolume;
                VolumeChargedAtFix = newVolume;
                //TotalDieselUsed += newVolume;
                newVolume = 0;
                
                return newVolume;
            }
            else
            {
                VolumeChargedAtFix = FixedVolumeRemainingForCurrent;
                //FixedVolumeUsedOnThisInvoice += FixedVolumeRemainingForCurrent;
                return newVolume - FixedVolumeRemainingForCurrent;
            }
        }
    }
}