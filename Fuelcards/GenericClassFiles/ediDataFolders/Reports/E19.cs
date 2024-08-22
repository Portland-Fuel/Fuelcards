using FuelcardModels.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelcardModels
{
    /// <summary>
    /// Cards Created, Reissued, Deactivated and Reactivated
    /// </summary>
    public class E19
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E19Detail> E19Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E19Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E19Detail
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordType RecordType{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 CustomerAccountCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 CustomerAccountSuffix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CardNumber PanNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime10 Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeOnly8 Time { get; set; }

        /// <summary>
        /// <para>4 = New card</para>
        /// <para>5 = Reissue card</para>
        /// <para>9 = Deactivated card</para>
        /// <para>R = Reactivated card</para>
        /// <para>B = Smartcard business rule changes</para>
        /// </summary>
        public ActionStatus ActionStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar OdometerUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VehicleRegistration VehicleReg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EmbossingDetails EmbossingDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 CardGrade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar MileageEntryFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar PinRequired { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int4 PinNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar TelephoneRequired { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int4 ExpiryDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar European { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar Smart { get; set; }

        /// <summary>
        /// This is an optional limit, will be 0 if the rule is not set
        /// </summary>
        public Int5 SingleTransFuelLimit { get; set; }

        /// <summary>
        ///  This is an optional limit, will be 0 if the rule is not set
        /// </summary>
        public Int7 DailyTransFuelLimit { get; set; }

        /// <summary>
        /// This is an optional limit, will be 0 if the rule is not set
        /// </summary>
        public Int7 WeeklyTransFuelLimit { get; set; }

        /// <summary>
        /// This is an optional limit, will be 0 if the rule is not set
        /// </summary>
        public Int5 NumberTransPerDay { get; set; }

        /// <summary>
        /// This is an optional limit, will be 0 if the rule is not set 
        /// </summary>
        public Int5 NumberTransPerWeek { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar NumberFalsePinEntries { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 PinLockoutMinutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar MondayAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar TueasdayAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar WednesdayAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar ThursdayAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar FridayAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar SaturdayAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar SundayAllowed { get; set; }

        /// <summary>
        /// Valid Start Time is 0 if this rule is not in place
        /// </summary>
        public Int4 ValidStartTime { get; set; }

        /// <summary>
        /// Valid End Time is 0 if this rule is not in place
        /// </summary>
        public Int4 ValidEndTime { get; set; }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E19Control
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public RecordType RecordType { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int6 CustomerAccountCode { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int2 CustomerAccountSuffix { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int9 Batchnumber { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public DateTime10 CreationDate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public TimeSpan8 CreationTime { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int5 RecordCount { get; set; }


    //}
}
