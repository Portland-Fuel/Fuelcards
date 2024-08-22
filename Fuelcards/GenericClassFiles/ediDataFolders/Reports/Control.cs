using System;
using FuelcardModels.Interfaces;
using FuelcardModels.DataTypes;

namespace FuelcardModels
{
    /// <summary>
    /// Generic Control Class for KeyFuels, UK Fuels and Texaco Reports
    /// </summary>
    public class Control
    {
        /// <summary>
        /// 
        /// </summary>
        public RecordType RecordType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IKfInt BatchNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICreationDate CreationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICreationTime CreationTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IKfInt CustomerCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 CustomerAC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IKfInt RecordCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IKfDouble TotalQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sign QuantitySign { get; set; }

        /// <summary>
        /// Either total cost or total value
        /// </summary>
        public IKfDouble TotalCost { get; set; }

        /// <summary>
        /// Either total cost or total value
        /// </summary>
        public Sign TotalCostSign { get; set; }
    }
}
