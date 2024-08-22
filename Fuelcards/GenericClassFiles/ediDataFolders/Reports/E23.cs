using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// New or closed sites
    /// </summary>
    public class E23
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E23Detail> E23Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E23Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E23Detail
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordType RecordType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 SiteAccountCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 SiteAccountSuffix { get; set; }

        /// <summary>
        /// <para>1 = New</para>
        /// <para>2 = Closed</para>
        /// <para>3 = Re-opened</para>
        /// </summary>
        public GenericChar SiteNewOrClosed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AddressLine1 AddressLine1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AddressLine2 AddressLine2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Town Town { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public County County { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PostCode PostCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TelephoneNumber TelephoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ContactName ContactName { get; set; }

        /// <summary>
        /// <para>Y = Is a retail site</para>
        /// <para>N = Not a retail site</para>
        /// </summary>
        public GenericChar RetailSite { get; set; }

        /// <summary>
        /// <para>Y = Has a canopy</para>
        /// <para>N = Does not have a canopy</para>
        /// </summary>
        public GenericChar Canopy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar MachineType { get; set; }

        /// <summary>
        /// eg MON-FRI = 24 Hours
        /// </summary>
        public OpeningHours1 OpeningHours1 { get; set; }

        /// <summary>
        /// eg SAT = 2400-1800
        /// </summary>
        public OpeningHours2 OpeningHours2 { get; set; }

        /// <summary>
        /// eg SUN = 0900-2400
        /// </summary>
        public OpeningHours3 OpeningHours3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Directions Directions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 PoleSignSupplier { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Parking { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar Payphone { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar GasOil { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Showers { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar OvernnightAccomodation { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar CafeRestaurant { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Toilets { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Shop { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Lubricants { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar SleeperCabsWelcome { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar TankCleaning { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Repairs { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar WindscreenReplacement { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar Bar { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar CashpointMachines { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar VehicleClearanceAccepted { get; set; }

        /// <summary>
        /// 1 = Available
        /// </summary>
        public GenericChar MotorwayJunction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 MotorwayNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 JunctionNumber { get; set; }


    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E23Control
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public RecordType RecordType { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int6 CustomerCode { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int2 CustomerAccountSuffix { get; set; }

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

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int9 BatchNumber { get; set; }
    //}
}
