using FuelcardModels;
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
    public class Texaco
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TexacoDetail> TexacoDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control TexacoControl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TexacoDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public Int3 Batch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int3 Division { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenericChar ClientType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Long10 Customer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Long10 Site { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateOnly6 TranDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeOnly4 TranTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int7 CardNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Registration Registration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int7 Mileage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int8 Quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 ProdNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int2 MonthNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int1 WeekNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int9 TranNoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int8 Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int6 IsoNumber { get; set; }
    }
}
