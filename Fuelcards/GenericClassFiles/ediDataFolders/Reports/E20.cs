using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Cards stopped and off stopped
    /// </summary>
    public class E20
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E20Detail> E20Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E20Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E20Detail
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordType RecordType { get; set; }

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
        public CardNumber PAN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime10 Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeOnly8 Time { get; set; }

        /// <summary>
        /// Must be a number between 1 and 9 if blank card is off stop
        /// <para>1 = Card lost</para>
        /// <para>2 = Card stolen</para>
        /// <para>3= Reason not given</para>
        /// <para>4 = Driver left</para>
        /// <para>5 = Card left at site</para>
        /// <para>6 = Card swapped at site</para>
        /// <para>7 = Card faulty</para>
        /// <para>8 = Account on credit stop</para>
        /// <para>9 = Account in liquidation</para>
        /// <para>blank  = Card off stop</para>
        /// </summary>
        public GenericChar StopCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Name PersonWhoRequestedStop { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int5 StopReferenceNumber { get; set; }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E20Control
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
    //    public Int9 BatchNumber { get; set; }

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
