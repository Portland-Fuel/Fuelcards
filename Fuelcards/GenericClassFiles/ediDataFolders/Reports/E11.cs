using System;
using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// 
    /// </summary>
    public class E11
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E11Detail> E11Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E11Control { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class E11Detail
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordType RecordType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int3 ProductCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProductDescription ProductDescription { get; set; }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E11Control
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public RecordType RecordType { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Int3 RecordCount { get; set; }

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
    //    public DateTime6 CreationDate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public TimeSpan6 CreationTime { get; set; }
    //}
}
