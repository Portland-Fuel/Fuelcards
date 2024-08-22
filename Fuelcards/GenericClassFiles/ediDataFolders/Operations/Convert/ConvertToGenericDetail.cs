using FuelcardModels;
using FuelcardModels.DataTypes;
using FuelcardModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Fuelcards;

using System.Threading.Tasks;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// Takes either KeyFuels, UK Fuels or Texaco Details and Converts it into a GenericDetail
    /// </summary>
    public class ConvertToGenericDetail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail">E01Detail</param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromKfE01Detail(E01Detail detail, int portlandId)
        {
            GenericDetail d = new GenericDetail();
            d.Network = (short)Network.KeyFuels;
            if (detail.CustomerCode.Value.HasValue)
                d.CustomerNo = new Long10(portlandId);
            if (detail.CardNumber.Value.HasValue)
                d.CardNumber = new CardNumber19(detail.CardNumber.Value.Value);
            if (detail.TransactionNumber.Value.HasValue)
                d.TransactionNo = new Int9(detail.TransactionNumber.Value.Value);
            if (!string.IsNullOrWhiteSpace(detail.PrimaryRegistration.Value))
                d.VehicleRegistration = detail.PrimaryRegistration;
            if (detail.Mileage.Value.HasValue)
                d.Mileage = detail.Mileage;
            if (detail.SiteCode.Value.HasValue)
                d.SiteNo = new Long10(detail.SiteCode.Value.Value);
            if (detail.TransactionDate.Value.HasValue)
                d.Date = detail.TransactionDate;
            if (detail.TransactionTime.Value.HasValue)
                d.Time = detail.TransactionTime;
            if (detail.ProductCode.Value.HasValue)
                d.ProductCode = detail.ProductCode;
            if (detail.Cost.Value.HasValue)
            {
                if (detail.CostSign != null && detail.CostSign.Value.HasValue && detail.CostSign.Value.Value == '-')
                {
                    d.Value = new Double11(detail.Cost.Value.Value * -1f);
                }
                else
                {
                    d.Value = new Double11(detail.Cost.Value.Value);
                }
            }

            if (detail.Quantity.Value.HasValue)
            {
                if (detail.QuantitySign != null && detail.QuantitySign.Value.HasValue && detail.QuantitySign.Value.Value == '-')
                {
                    d.Quantity = new Double11(detail.Quantity.Value.Value * -1f);
                }
                else
                {
                    d.Quantity = detail.Quantity;
                }
            }

            if (d.Value.Value.HasValue && d.Quantity.Value.HasValue)
                d.Price = new Double11(d.Value.Value.Value / d.Quantity.Value.Value);

            return d;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromKfE03Detail(E03Detail detail, int portlandId)
        {
            GenericDetail d = new GenericDetail();
            d.Network = (short)Network.KeyFuels;
            if (detail.CustomerCode.Value.HasValue)
                d.CustomerNo = new Long10(portlandId);
            if (detail.CardNumber.Value.HasValue)
                d.CardNumber = new CardNumber19(detail.CardNumber.Value.Value);
            if (detail.TransactionNumber.Value.HasValue)
                d.TransactionNo = new Int9(detail.TransactionNumber.Value.Value);
            if (!string.IsNullOrWhiteSpace(detail.PrimaryRegistration.Value))
                d.VehicleRegistration = new VehicleRegistration(detail.PrimaryRegistration.Value);
            if (detail.Mileage.Value.HasValue)
                d.Mileage = detail.Mileage;
            if (detail.SiteCode.Value.HasValue)
                d.SiteNo = new Long10(detail.SiteCode.Value.Value);
            if (detail.TransactionDate.Value.HasValue)
                d.Date = detail.TransactionDate;
            if (detail.TransactionTime.Value.HasValue)
                d.Time = detail.TransactionTime;
            if (detail.ProductCode.Value.HasValue)
                d.ProductCode = detail.ProductCode;
            if (detail.Value.Value.HasValue)
            {
                if (detail.ValueSign != null && detail.ValueSign.Value.HasValue && detail.ValueSign.Value.Value == '-')
                {
                    d.Value = new Double11(detail.Value.Value.Value * -1);
                }
                else
                {
                    d.Value = new Double11(detail.Value.Value.Value);
                }
            }

            if (detail.Quantity.Value.HasValue)
            {
                if (detail.QuantitySign != null && detail.QuantitySign.Value.HasValue && detail.QuantitySign.Value.Value == '-')
                {
                    d.Quantity = new Double11(detail.Quantity.Value.Value * -1);
                }
                else
                {
                    d.Quantity = new Double11(detail.Quantity.Value.Value);
                }
            }

            if (d.Value.Value.HasValue && d.Quantity.Value.HasValue)
                d.Price = new Double11(d.Value.Value.Value / d.Quantity.Value.Value);

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromKUKFuelsDetail(UKFuelsDetail detail, int portlandId)
        {
            GenericDetail d = new GenericDetail();

            d.Network = (short)Network.UkFuels;
            if (detail.Customer.Value.HasValue)
                d.CustomerNo = new Long10(portlandId);
            if (detail.PanNumber.Value.HasValue)
                d.CardNumber = new CardNumber20(detail.PanNumber.Value.Value);
            if (detail.TranNoItem.Value.HasValue)
                d.TransactionNo = detail.TranNoItem;
            if (!string.IsNullOrWhiteSpace(detail.Registration.Value))
                d.VehicleRegistration = new VehicleRegistration(detail.Registration.Value);
            if (detail.Mileage.Value.HasValue)
                d.Mileage = detail.Mileage;
            if (detail.Site.Value.HasValue)
                d.SiteNo = detail.Site;
            if (detail.TranDate.Value.HasValue)
                d.Date = new DateTime10(detail.TranDate.Value.Value);
            if (detail.TranTime.Value.HasValue)
                d.Time = new TimeOnly8(detail.TranTime.Value.Value);
            if (detail.ProdNo.Value.HasValue)
                d.ProductCode = new Int3(detail.ProdNo.Value.Value);
            if (detail.Price.Value.HasValue)
                d.Price = new Double11(detail.Price.Value.Value / 100f);

            if (detail.Quantity.Value.HasValue)
                d.Quantity = new Double11(detail.Quantity.Value.Value / 100f);

            if (d.Price.Value.HasValue && d.Quantity.Value.HasValue)
                d.Value = new Double11(d.Price.Value.Value * d.Quantity.Value.Value);

            return d;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromKfTexacoDetail(TexacoDetail detail, int portlandId)
        {
            GenericDetail d = new GenericDetail();

            d.Network = (short)Network.Texaco;
            if (detail.Customer.Value.HasValue)
                d.CustomerNo = new Long10(portlandId);
            if (detail.CardNo.Value.HasValue)
                d.CardNumber = new CardNumber20(detail.CardNo.Value.Value);
            if (detail.TranNoItem.Value.HasValue)
                d.TransactionNo = detail.TranNoItem;
            if (!string.IsNullOrWhiteSpace(detail.Registration.Value))
                d.VehicleRegistration = new VehicleRegistration(detail.Registration.Value);
            if (detail.Mileage.Value.HasValue)
                d.Mileage = detail.Mileage;
            if (detail.Site.Value.HasValue)
                d.SiteNo = detail.Site;
            if (detail.TranDate.Value.HasValue)
                d.Date = new DateTime10(detail.TranDate.Value.Value);
            if (detail.TranTime.Value.HasValue)
                d.Time = new TimeOnly8(detail.TranTime.Value.Value);
            if (detail.ProdNo.Value.HasValue)
                d.ProductCode = new Int3(detail.ProdNo.Value.Value);
            if (detail.Price.Value.HasValue)
                d.Price = new Double11(detail.Price.Value.Value / 100f);

            if (detail.Quantity.Value.HasValue)
                d.Quantity = new Double11(detail.Quantity.Value.Value / 100f);

            if (d.Price.Value.HasValue && d.Quantity.Value.HasValue)
                d.Value = new Double11(d.Price.Value.Value * d.Quantity.Value.Value);

            return d;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromKfE01E03Db(KfE1E3Transaction db, int portlandId)
        {
            GenericDetail d = new GenericDetail();

            d.Network = (short)Network.KeyFuels;
            if (!DBNull.Value.Equals(db.PortlandId))
                d.CustomerNo = new Long10(db.PortlandId.Value);
            if (!DBNull.Value.Equals(db.CardNumber))
                d.CardNumber = new CardNumber19(db.CardNumber.Value);
            if (!DBNull.Value.Equals(db.TransactionNumber))
                d.TransactionNo = new Int9(db.TransactionNumber.Value);
            if (!DBNull.Value.Equals(db.PrimaryRegistration))
                d.VehicleRegistration = new VehicleRegistration(db.PrimaryRegistration);
            if (!DBNull.Value.Equals(db.Mileage))
                d.Mileage = new Int7(db.Mileage.Value);
            if (!DBNull.Value.Equals(db.SiteCode))
                d.SiteNo = new Long10(db.SiteCode.Value);
            if (!DBNull.Value.Equals(db.TransactionDate))
                d.Date = new DateTime10(db.TransactionDate.Value);
            if (!DBNull.Value.Equals(db.TransactionTime))
                d.Time = new TimeOnly8(db.TransactionTime.Value);
            if (!DBNull.Value.Equals(db.ProductCode))
                d.ProductCode = new Int3(db.ProductCode.Value);
            if (!DBNull.Value.Equals(db.Quantity))
                d.Quantity = new Double11(db.Quantity.Value);
            if (!DBNull.Value.Equals(db.Cost))
                d.Value = new Double11(db.Cost.Value);
            if (d.Quantity.Value.HasValue && d.Value.Value.HasValue)
                d.Price = new Double11(d.Value.Value.Value / d.Quantity.Value.Value);

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uk"></param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromUKFuelsDb(UkfTransaction uk, int portlandId)
        {
            GenericDetail d = new GenericDetail();

            d.Network = (short)Network.UkFuels;
            
            if (!DBNull.Value.Equals(uk.PortlandId))
                d.CustomerNo = new Long10(uk.PortlandId.Value);
            
            if (!DBNull.Value.Equals(uk.PanNumber))
                d.CardNumber = new CardNumber19(uk.PanNumber.Value);
            
            if (!DBNull.Value.Equals(uk.TranNoItem))
                d.TransactionNo = new Int9(uk.TranNoItem.Value);
            
            if (!DBNull.Value.Equals(uk.Registration))
                d.VehicleRegistration = new VehicleRegistration(uk.Registration);
            
            if (!DBNull.Value.Equals(uk.Mileage))
                d.Mileage = new Int7(uk.Mileage.Value);
            
            if (!DBNull.Value.Equals(uk.Site))
                d.SiteNo = new Long10(uk.Site.Value);
            
            if (!DBNull.Value.Equals(uk.TranDate))
                d.Date = new DateTime10(uk.TranDate.Value);
            
            if (!DBNull.Value.Equals(uk.TranTime))
                d.Time = new TimeOnly8(uk.TranTime.Value);
            
            if (!DBNull.Value.Equals(uk.Quantity))
                d.Quantity = new Double11(uk.Quantity.Value / 100f); 
            
            if (!DBNull.Value.Equals(uk.ProdNo))
            {
                d.ProductCode = new Int3(uk.ProdNo.Value);

                if(d.ProductCode.Value == 1)
                {
                    if (!DBNull.Value.Equals(uk.Price))
                        d.Price = new Double11(uk.Price.Value / 100f);

                    if (d.Quantity.Value.HasValue && d.Price.Value.HasValue)
                        d.Value = new Double11(d.Price.Value.Value * d.Quantity.Value.Value);
                }
                else
                {
                    if (!DBNull.Value.Equals(uk.Price))
                        d.Value = new Double11(uk.Price.Value / 100f);

                    if (d.Quantity.Value.HasValue && d.Value.Value.HasValue)
                        d.Price = new Double11(d.Value.Value.Value / d.Quantity.Value.Value);
                }
            }

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="portlandId">int</param>
        /// <returns></returns>
        public static GenericDetail FromTexacoDb(TexacoTransaction t, int portlandId)
        {
            GenericDetail d = new GenericDetail();

            d.Network = (short)Network.Texaco;
            if (!DBNull.Value.Equals(t.PortlandId))
                d.CustomerNo = new Long10(t.PortlandId.Value);
            if (!DBNull.Value.Equals(t.CardNo))
                d.CardNumber = new CardNumber19(t.CardNo.Value);
            if (!DBNull.Value.Equals(t.TranNoItem))
                d.TransactionNo = new Int9(t.TranNoItem.Value);
            if (!DBNull.Value.Equals(t.Registration))
                d.VehicleRegistration = new VehicleRegistration(t.Registration);
            if (!DBNull.Value.Equals(t.Mileage))
                d.Mileage = new Int7(t.Mileage.Value);
            if (!DBNull.Value.Equals(t.Site))
                d.SiteNo = new Long10(t.Site.Value);
            if (!DBNull.Value.Equals(t.TranDate))
                d.Date = new DateTime10(t.TranDate.Value);
            if (!DBNull.Value.Equals(t.TranTime))
                d.Time = new TimeOnly8(t.TranTime.Value);
            if (!DBNull.Value.Equals(t.ProdNo))
                d.ProductCode = new Int3(t.ProdNo.Value);
            if (!DBNull.Value.Equals(t.Quantity))
                d.Quantity = new Double11(t.Quantity.Value / 100);
            if (!DBNull.Value.Equals(t.Price))
                d.Price = new Double11(t.Price.Value / 100f);
            if (d.Quantity.Value.HasValue && d.Price.Value.HasValue)
                d.Value = new Double11(d.Price.Value.Value * d.Quantity.Value.Value);

            return d;
        }

        /// <summary>
        /// Takes a GenericDetail and converts it to a string
        /// </summary>
        /// <param name="d"></param>
        /// <returns>string for the detail provided</returns>
        public static string GenericDetailToString(GenericDetail d)
        {
            string line = d.Network + ",";
            if (d.CustomerNo.Value.HasValue) line += d.CustomerNo.Text;
            line += ",";
            if (d.CardNumber.Value.HasValue) line += d.CardNumber.Text;
            line += ",";
            if (d.TransactionNo.Value.HasValue) line += d.TransactionNo.Text;
            line += ",";
            if (d.VehicleRegistration != null && !string.IsNullOrWhiteSpace(d.VehicleRegistration.Value)) line += d.VehicleRegistration.ToString();
            line += ",";
            if (d.Mileage.Value.HasValue) line += d.Mileage.Text;
            line += ",";
            if (d.SiteNo.Value.HasValue) line += d.SiteNo.Text;
            line += ",";
            if (d.Date.Value.HasValue) line += d.Date.Text;
            line += ",";
            if (d.Time.Value.HasValue) line += d.Time.Text;
            line += ",";
            if (d.ProductCode.Value.HasValue) line += d.ProductCode.Text;
            line += ",";
            if (d.Price.Value.HasValue) line += d.Price.Text;
            line += ",";
            if (d.Quantity.Value.HasValue) line += d.Quantity.Text;
            line += ",";
            if (d.Value.Value.HasValue) line += d.Value.Text;

            return line;
        }

        /// <summary>
        /// Takes a GenericDetail and converts it to a string
        /// </summary>
        /// <param name="d"></param>
        /// <returns>string for the detail provided</returns>
        public static GenericDetail StringToGenericDetail(string d)
        {
            if(d.Length == 0) return null; 
            string[] parts = d.Split(',');
            if(parts.Length != 13) return null;


            if (parts[0] == "d") return null;
            GenericDetail gd = new GenericDetail();


            if (short.TryParse(parts[0], out short network)) gd.Network = network;
            if (long.TryParse(parts[1], out long customerno)) gd.CustomerNo = new Long10(parts[1]);
            ICardNumber cardNo = GetCardNumberFromString(parts[2]);
            if(cardNo is not null) gd.CardNumber = cardNo;

            if (int.TryParse(parts[3], out int transactionNo)) gd.TransactionNo = new Int9(transactionNo);
            gd.VehicleRegistration = new VehicleRegistration(parts[4]);
            if (int.TryParse(parts[5], out int mileage)) gd.Mileage = new Int7(mileage);
            if (long.TryParse(parts[6], out long siteNo)) gd.SiteNo = new Long10(siteNo);
            if (parts[7].Length == 10) gd.Date = new DateTime10(parts[7]);
            if (parts[8].Length == 8) gd.Time = new TimeOnly8(parts[8]);

            if (int.TryParse(parts[9], out int productCode)) gd.ProductCode = new Int3(productCode);
            if(double.TryParse(parts[10], out double price)) gd.Price = new Double11(parts[10]);
            if (double.TryParse(parts[11], out double quantity)) gd.Quantity = new Double11(parts[11]);
            if (double.TryParse(parts[12], out double valueDbl)) gd.Value = new Double11(parts[12]);
            

            return gd;
        }

        private static ICardNumber GetCardNumberFromString(string cardNumber)
        {
            if (cardNumber.Length == 7) return new CardNumber7(cardNumber);
            if (cardNumber.Length == 19) return new CardNumber19(cardNumber);
            if(cardNumber.Length == 20) return new CardNumber20(cardNumber);
            return null;
        }
    }
}
