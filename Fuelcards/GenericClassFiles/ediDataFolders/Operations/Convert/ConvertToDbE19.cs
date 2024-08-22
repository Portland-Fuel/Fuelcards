using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE19
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E19Detail"></param>
        /// <returns></returns>
        public static KfE19Card FileToDb(E19Detail E19Detail)
        {
            KfE19Card d = new KfE19Card();


            if (E19Detail.CustomerAccountCode.Value.HasValue) d.CustomerAccountCode = E19Detail.CustomerAccountCode.Value.Value;
            if (E19Detail.CustomerAccountSuffix.Value.HasValue) d.CustomerAccountSuffix = (short)E19Detail.CustomerAccountSuffix.Value.Value;
            if (E19Detail.PanNumber.Value.HasValue) d.PanNumber = E19Detail.PanNumber.Value;
            if (E19Detail.Date.Value.HasValue) d.Date = E19Detail.Date.Value.Value;
            if (E19Detail.Time.Value.HasValue) d.Time = E19Detail.Time.Value.Value;
            if (!string.IsNullOrWhiteSpace(E19Detail.ActionStatus.Text)) d.ActionStatus = E19Detail.ActionStatus.Text;
            if (!string.IsNullOrWhiteSpace(E19Detail.OdometerUnit.Text)) d.ActionStatus = E19Detail.ActionStatus.Text;
            if (!string.IsNullOrWhiteSpace(E19Detail.VehicleReg.Value)) d.VehicleReg = E19Detail.VehicleReg.ToString();
            if (!string.IsNullOrWhiteSpace(E19Detail.EmbossingDetails.Value)) d.VehicleReg = E19Detail.VehicleReg.ToString();
            if (E19Detail.CardGrade.Value.HasValue) d.CardGrade = E19Detail.CardGrade.Value.Value;
            if (!string.IsNullOrWhiteSpace(E19Detail.MileageEntryFlag.Text)) d.MileageEntryFlag = E19Detail.MileageEntryFlag.Text;
            d.PinRequired = (E19Detail.PinRequired.Text.ToLower() == "y") ? true : false ;
            if (E19Detail.PinNumber.Value.HasValue) d.PinNumber = (short)E19Detail.PinNumber.Value.Value;
            d.TelephoneRequired = (E19Detail.TelephoneRequired.Text.ToLower() == "y") ? true : false ;
            if (E19Detail.ExpiryDate.Value.HasValue) d.ExpiryDate = (short)E19Detail.ExpiryDate.Value.Value;
            d.European = (E19Detail.European.Text.ToLower() == "y") ? true : false ;
            d.Smart = (E19Detail.Smart.Text.ToLower() == "y") ? true : false;
            if (E19Detail.SingleTransFuelLimit.Value.HasValue) d.SingleTransFuelLimit = E19Detail.SingleTransFuelLimit.Value.Value;
            if (E19Detail.DailyTransFuelLimit.Value.HasValue) d.DailyTransFuelLimit = E19Detail.DailyTransFuelLimit.Value.Value;
            if (E19Detail.WeeklyTransFuelLimit.Value.HasValue) d.WeeklyTransFuelLimit = E19Detail.WeeklyTransFuelLimit.Value.Value;
            if (E19Detail.NumberTransPerDay.Value.HasValue) d.NumberTransPerDay = E19Detail.NumberTransPerDay.Value.Value;
            if (E19Detail.NumberTransPerWeek.Value.HasValue) d.NumberTransPerWeek = E19Detail.NumberTransPerWeek.Value.Value;
            if (E19Detail.PinLockoutMinutes.Value.HasValue) d.PinLockoutMinutes = E19Detail.PinLockoutMinutes.Value.Value;
            d.MondayAllowed = (E19Detail.MondayAllowed.Text.ToLower() == "n") ? false : true;
            d.TuesdayAllowed = (E19Detail.TueasdayAllowed.Text.ToLower() == "n") ? false : true;
            d.WednesdayAllowed = (E19Detail.WednesdayAllowed.Text.ToLower() == "n") ? false : true;
            d.ThursdayAllowed = (E19Detail.ThursdayAllowed.Text.ToLower() == "n") ? false : true;
            d.FridayAllowed = (E19Detail.FridayAllowed.Text.ToLower() == "n") ? false : true;
            d.SaturdayAllowed = (E19Detail.SaturdayAllowed.Text.ToLower() == "n") ? false : true;
            d.SundayAllowed = (E19Detail.SundayAllowed.Text.ToLower() == "n") ? false : true;
            if (E19Detail.ValidStartTime.Value.HasValue) d.ValidStartTime = (short)E19Detail.ValidStartTime.Value.Value;
            if (E19Detail.ValidEndTime.Value.HasValue) d.ValidEndTime = (short)E19Detail.ValidEndTime.Value.Value;

            return d;
        }
    }
}
