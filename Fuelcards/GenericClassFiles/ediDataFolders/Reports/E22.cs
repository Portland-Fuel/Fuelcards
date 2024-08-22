using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Accounts Stopped or Off Stopped
    /// </summary>
    public class E22
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E22Detail> E22Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E22Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E22Detail
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
        public DateTime10 Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeOnly8 Time { get; set; }

        /// <summary>
        /// Shows the reason the account is stopped 
        /// <para>A = Dealer request</para>
        /// <para>B = Bankruptcy</para>
        /// <para>L = Liquidation</para>
        /// <para>N = Not acknowledge</para>
        /// <para>O = Out of fuel</para>
        /// <para>P = Payment</para>
        /// <para>R = Receivership</para>
        /// <para>U = Unknown</para>
        /// <para>blank = off stop</para>
        /// </summary>
        public GenericChar StopStatusCode { get; set; }


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
    //public class E22Control
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
    //    public DateTime10 CreationDate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public TimeSpan8 CreationTime { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int9 BatchNumber { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int5 RecordCount { get; set; }
    //}
}
