using FuelcardModels.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelcardModels
{
    /// <summary>
    /// 
    /// </summary>
    public class E05
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E05Detail> E05Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E05Control { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class E05Detail
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordType RecordType { get; set; }

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
        public Int3 ProductCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 OpeningStockBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign OpeningBalanceSign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 DrawingQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign DrawingQuantitySign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int7 NumberOfDrawings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 DeliveryQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign DeliveryQuantitySign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int5 NumberOfDeliveries { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 ClosingStockBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign ClosingBalanceSign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int4 Period { get; set; }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E05Control
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
    //    public Int2 CustomerAC { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public BatchNumber BatchNumber { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int5 RecordCount { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public DateTime6 CreationDate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public TimeSpan6 CreationTime { get; set; }


    //}
}
