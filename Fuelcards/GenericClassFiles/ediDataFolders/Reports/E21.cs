using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Accounts created or deactivated
    /// </summary>
    public class E21
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E21Detail> E21Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E21Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E21Detail
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
        /// 
        /// </summary>
        public ActionStatus ActionStatus { get; set; }

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
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E21Control
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
