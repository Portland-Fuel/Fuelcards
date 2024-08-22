using System;
using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Drawing Details
    /// </summary>
    public class E01
    {
        /// <summary>
        /// List containing the E01 Detail from each line of the EDI
        /// </summary>
        public List<E01Detail> E01Details { get; set; }

        /// <summary>
        /// Contain the data from the Control Record
        /// </summary>
        public Control E01Control { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class E01Detail
    {
        /// <summary>
        /// E01 Detail Record for the Extended Version
        /// </summary>
        public RecordType RecordType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int9 TransactionNumber{ get; set; }
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
        public Int2 PumpNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CardNumber CardNumber { get; set; }

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
        public VehicleRegistration PrimaryRegistration { get; set; }

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
        public Double11 Quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign QuantitySign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double7 Cost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign CostSign { get; set; }

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
    ///// E01 Control Record for the Extended Version
    ///// </summary>
    //public class E01Control
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
    //    public Double11 TotalQuantity { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Sign QuantitySign { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Double7 TotalCost { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Sign TotalCostSign { get; set; }

    //}
}
