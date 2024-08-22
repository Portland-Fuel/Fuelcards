
using System;
using DataAccess.Fuelcards;

using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConvertToDbE23
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E23Detail"></param>
        /// <returns></returns>
        public static KfE23NewClosedSite FileToDb(E23Detail E23Detail)
        {
            KfE23NewClosedSite d = new KfE23NewClosedSite();


            if(E23Detail.SiteAccountCode.Value.HasValue) d.SiteAccountCode = E23Detail.SiteAccountCode.Value.Value;
            if (E23Detail.SiteAccountSuffix.Value.HasValue) d.SiteAccountSuffix = E23Detail.SiteAccountSuffix.Value.Value;
            d.SiteStatus = E23Detail.SiteNewOrClosed.Value.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.Name.Value)) d.Name = E23Detail.Name.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.AddressLine1.Value)) d.AddressLine1 = E23Detail.AddressLine1.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.AddressLine2.Value)) d.AddressLine2 = E23Detail.AddressLine2.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.Town.Value)) d.Town = E23Detail.Town.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.County.Value)) d.County = E23Detail.County.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.PostCode.Value)) d.Postcode = E23Detail.PostCode.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.TelephoneNumber.Value)) d.TelephoneNumber = E23Detail.TelephoneNumber.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.ContactName.Value)) d.ContactName = E23Detail.ContactName.ToString();
            if (E23Detail.RetailSite.Value == '1') d.RetailSite = true; else d.RetailSite = false;
            if (E23Detail.Canopy.Value == '1') d.Canopy = true; else d.Canopy = false;
            d.MachineType = E23Detail.MachineType.Value.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.OpeningHours1.Value)) d.OpeningHours1 = E23Detail.OpeningHours1.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.OpeningHours2.Value)) d.OpeningHours2 = E23Detail.OpeningHours2.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.OpeningHours3.Value)) d.OpeningHours3 = E23Detail.OpeningHours3.ToString();
            if (!string.IsNullOrWhiteSpace(E23Detail.Directions.Value)) d.Directions = E23Detail.Directions.ToString();
            if (E23Detail.PoleSignSupplier.Value.HasValue) d.PoleSignSupplier = E23Detail.PoleSignSupplier.Value.Value;
            if (E23Detail.Parking.Value == '1') d.Parking = true; else d.Parking = false;
            if (E23Detail.Payphone.Value == '1') d.Payphone = true; else d.Payphone = false;
            if (E23Detail.GasOil.Value == '1') d.Gasoil = true; else d.Gasoil = false;
            if (E23Detail.Showers.Value == '1') d.Showers = true; else d.Showers = false;
            if (E23Detail.OvernnightAccomodation.Value == '1') d.OvernightAccomodation = true; else d.OvernightAccomodation = false;
            if (E23Detail.CafeRestaurant.Value == '1') d.CafeRestaurant = true; else d.CafeRestaurant = false;
            if (E23Detail.Toilets.Value == '1') d.Toilets = true; else d.Toilets = false;
            if (E23Detail.Shop.Value == '1') d.Shop = true; else d.Shop = false;
            if (E23Detail.Lubricants.Value == '1') d.Lubricants = true; else d.Lubricants = false;
            if (E23Detail.SleeperCabsWelcome.Value == '1') d.SleeperCabsWelcome = true; else d.SleeperCabsWelcome = false;
            if (E23Detail.TankCleaning.Value == '1') d.TankCleaning = true; else d.TankCleaning = false;
            if (E23Detail.Repairs.Value == '1') d.Repairs = true; else d.Repairs = false;
            if (E23Detail.WindscreenReplacement.Value == '1') d.WindscreenReplacement = true; else d.WindscreenReplacement = false;
            if (E23Detail.Bar.Value == '1') d.Bar = true; else d.Bar = false;
            if (E23Detail.CashpointMachines.Value == '1') d.CashpointMachines = true; else d.CashpointMachines = false;
            if (E23Detail.VehicleClearanceAccepted.Value == '1') d.VehicleClearanceAccepted = true; else d.VehicleClearanceAccepted = false;
            if (E23Detail.MotorwayJunction.Value == '1') d.MotorwayJunction = true; else d.MotorwayJunction = false;
            if (E23Detail.MotorwayNumber.Value.HasValue) d.MotorwayNumber = E23Detail.MotorwayNumber.Value;
            if (E23Detail.JunctionNumber.Value.HasValue) d.JunctionNumber = E23Detail.JunctionNumber.Value;


            return d;

        }

    }
}
