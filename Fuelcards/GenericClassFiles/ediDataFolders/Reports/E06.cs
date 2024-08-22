using System;
using System.Collections.Generic;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// E06 
    /// </summary>
    public class E06
    {
        /// <summary>
        /// 
        /// </summary>
        public List<E06Detail>  E06Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control E06Control { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class E06Detail
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
        public Narrative Narrative { get; set; }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class E06Control
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
