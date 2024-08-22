using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuelcardModels;
using FuelcardModels.DataTypes;
using FuelcardModels.Interfaces;
using FuelcardModels.Operations;

namespace FuelcardModels
{
    /// <summary>
    /// Creates the transaction report to send to Driving Down for all Networks
    /// </summary>
    public class GenericTransactionReport
    {
        private List<GenericDetail> _details;
        private Control _c;
        private int _introducersId;

        /// <summary>
        /// 
        /// </summary>
        public int IntroducersId { get { return _introducersId; } }
        /// <summary>
        /// 
        /// </summary>
        public List<GenericDetail> DrivingDownDetails { get { return _details; } }

        /// <summary>
        /// 
        /// </summary>
        public Control DrivingDownControl { get { return _c; }  }

        /// <summary>
        /// 
        /// </summary>
        public GenericTransactionReport(int IntroducersId)
        {
            _introducersId = IntroducersId;
            _details = new List<GenericDetail>();
        }

        /// <summary>
        /// Adds a GenericDetail to the listof Details
        /// </summary>
        /// <param name="Detail"></param>
        public void Add(GenericDetail Detail)
        {
            _details.Add(Detail);
        }

        /// <summary>
        /// Removes a record from the Details
        /// </summary>
        /// <param name="Detail"></param>
        public void Remove(GenericDetail Detail)
        {
            _details.Remove(Detail);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CreateControl()
        {
            if (_details.Count < 1) return false;

            _c = new Control();

            _c.RecordType = new RecordType('d');
            _c.CreationDate = new DateTime10(DateOnly.FromDateTime(DateTime.Today));
            _c.CreationTime = new TimeOnly8(TimeOnly.FromDateTime(DateTime.Now));
            _c.CustomerCode = new Int6(_introducersId);
            _c.RecordCount = new Int6(_details.Count);
            _c.TotalQuantity = new Double11(_details.Sum(d => d.Quantity.Value.Value));
            _c.TotalCost = new Double11(_details.Sum(d => d.Value.Value.Value));
            return true;
        }

        /// <summary>
        /// If the Control doesn't exist, it creates it then outputs the line as a string
        /// </summary>
        /// <returns></returns>
        public string ControlToString()
        {
            if (_c == null) if(!CreateControl()) return string.Empty;
            string line = "d,";
            line += _c.CustomerCode.Text + ",,,,,";
            line += _c.RecordCount.Text + ",";
            line += _c.CreationDate.Text + ",";
            line += _c.CreationTime.Text + ",,,";
            line += _c.TotalQuantity.Text + ",";
            line += _c.TotalCost.Text;

            return line;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DetailsToString()
        {
            if (_details.Count < 1) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var d in _details)
            {
                if (sb.Length == 0)
                {
                    sb.Append(ConvertToGenericDetail.GenericDetailToString(d));
                }
                else 
                {
                    sb.Append(Environment.NewLine + ConvertToGenericDetail.GenericDetailToString(d));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the report as a string
        /// </summary>
        /// <returns></returns>
        public string ReportToString()
        {
            if (_details.Count < 1) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var d in _details)
            {
                sb.Append(ConvertToGenericDetail.GenericDetailToString(d) + Environment.NewLine);
            }
            sb.Append(ControlToString());
            return sb.ToString();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class GenericDetail
    {
        /// <summary>
        /// Enum 
        /// <para>0 - Keyfuels</para>
        /// <para>1 - UK Fuels</para>
        /// <para>2 - Texaco / FastFuels</para>
        /// </summary>
        public short Network { get; set; }

        /// <summary>
        /// Takes an ICardNumber as the PAN number
        /// </summary>
        public ICardNumber CardNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Long10 CustomerNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int9 TransactionNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VehicleRegistration VehicleRegistration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int7 Mileage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Long10 SiteNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime10 Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeOnly8 Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int3 ProductCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double11 Quantity { get; set; }

    }
    
}
