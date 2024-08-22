using FuelcardModels.DataTypes;
using System;
using System.Collections.Generic;


namespace FuelcardModels
{
    /// <summary>
    /// Sale Sundry Details
    /// </summary>
    public class E04
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E04Detail> E04Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E04Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E04Detail
    {
        /// <summary>
        /// E04 Detail Record for the Extended Version
        /// </summary>
        public RecordType RecordType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int9 TransactionNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int3 TransactionSequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime10 TransactionDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeOnly8 TransactionTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 CustomerCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 CustomerAC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int4 Period { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int3 ProductCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double9 Quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign QuantitySign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign ValueSign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CardNumber CardNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VehicleRegistration VehicleRegistration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Reference Reference { get; set; }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E04Control
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public RecordType RecordType { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public BatchNumber BatchNumber { get; set; }

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
    //    public Int6 CustomerCode { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int2 CustomerAC { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Double9 TotalQuantity { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Sign QuantitySign { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Double11 TotalValue { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Sign ValueSign { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int5 RecordCount { get; set; }
    //}
}
