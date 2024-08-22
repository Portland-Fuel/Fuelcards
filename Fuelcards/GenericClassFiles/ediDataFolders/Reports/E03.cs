using System;
using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Retail Drawinngs And Site Sundry Details
    /// </summary>
    public class E03
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E03Detail> E03Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E03Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E03Detail
    {
        /// <summary>
        /// 
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
        public Int4 Period { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 SiteCode { get; set; }

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
        public CardNumber CardNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PrimaryRegistration PrimaryRegistration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int7 Mileage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 FleetNumber { get; set; }

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
        public AccurateMileage AccurateMileage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CardRegistration CardRegistration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VehicleRegistration TransactionRegistration { get; set; }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E03Control
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
    //    public Int7 RecordCount { get; set; }

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

    //}
}
