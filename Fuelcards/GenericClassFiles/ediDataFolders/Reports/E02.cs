using System;
using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Delivery And Transfer Details
    /// </summary>
    public class E02
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E02Detail> E02Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E02Control { get; set; }

    }

    /// <summary>
    /// Contains a sigle row of data from the detail record of E02 EDI
    /// </summary>
    public class E02Detail
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
        public SupplierName SupplierName { get; set; }

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
        public CustOwnOrderNo CustOwnOrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomerOrderNo CustomerOrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HandlingCharge HandlingCharge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeliveryNoteNo DeliveryNoteNo { get; set; }
    }

    ///// <summary>
    ///// E02 Control contains the data from the control record
    ///// </summary>
    //public class E02Control
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
    //    public Double11 TotalQuantity { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Sign QuantitySign { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int7 RecordCount { get; set; }
    //}
}
