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
    /// List containing the details of the UK Fuels EDI file
    /// </summary>
    public class UKFuels
    {
        /// <summary>
        /// 
        /// </summary>
        public List<UKFuelsDetail> UKFuelsDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Control UKFuelsControl { get; set; }
    }
    /// <summary>
    /// UK Fuels EDI Report
    /// </summary>
    public class UKFuelsDetail
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
        public ReceiptNo ReceiptNo { get; set; }

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
        public Decimal20 PanNumber { get; set; }

    }


}
