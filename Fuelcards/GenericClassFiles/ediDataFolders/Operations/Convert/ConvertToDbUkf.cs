using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelCardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbUkf
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UkfDetail"></param>
        /// <returns></returns>
        public static UkfTransaction FileToDb(UKFuelsDetail UkfDetail)
        {
            UkfTransaction u = new UkfTransaction();

            if (UkfDetail.Batch is not null && UkfDetail.Batch.Value.HasValue) u.Batch = (short)UkfDetail.Batch.Value.Value;
            if (UkfDetail.CardNo is not null && UkfDetail.CardNo.Value.HasValue) u.Batch = (short)UkfDetail.CardNo.Value.Value;
            if (UkfDetail.ClientType is not null && UkfDetail.ClientType.Value.HasValue) u.ClientType = UkfDetail.ClientType.Value.Value;
            if (UkfDetail.Customer is not null && UkfDetail.Customer.Value.HasValue) u.Customer = UkfDetail.Customer.Value.Value;
            if (UkfDetail.Division is not null && UkfDetail.Division.Value.HasValue) u.Division = (short)UkfDetail.Division.Value.Value;
            if (UkfDetail.Mileage is not null && UkfDetail.Mileage.Value.HasValue) u.Mileage = UkfDetail.Mileage.Value.Value;
            if (UkfDetail.MonthNo is not null && UkfDetail.MonthNo.Value.HasValue) u.MonthNo = UkfDetail.MonthNo.Value.Value;
            if (UkfDetail.PanNumber is not null && UkfDetail.PanNumber.Value.HasValue) u.PanNumber = UkfDetail.PanNumber.Value.Value;
            if (UkfDetail.Price is not null && UkfDetail.Price.Value.HasValue) u.Price = UkfDetail.Price.Value.Value;
            if (UkfDetail.ProdNo is not null && UkfDetail.ProdNo.Value.HasValue) u.ProdNo = UkfDetail.ProdNo.Value.Value;
            if (UkfDetail.Quantity is not null && UkfDetail.Quantity.Value.HasValue) u.Quantity = UkfDetail.Quantity.Value.Value;
            if (UkfDetail.ReceiptNo is not null && string.IsNullOrWhiteSpace(UkfDetail.ReceiptNo.Value)) u.ReceiptNo = UkfDetail.ReceiptNo.ToString();
            if (UkfDetail.Registration is not null) u.Registration = UkfDetail.Registration.Value;
            if (UkfDetail.Site is not null && UkfDetail.Site.Value.HasValue) u.Site = UkfDetail.Site.Value.Value;
            if (UkfDetail.TranDate is not null && UkfDetail.TranDate.Value.HasValue) u.TranDate = UkfDetail.TranDate.Value.Value;
            if (UkfDetail.TranTime is not null && UkfDetail.TranTime.Value.HasValue) u.TranTime = UkfDetail.TranTime.Value.Value;
            if (UkfDetail.TranNoItem is not null && UkfDetail.TranNoItem.Value.HasValue) u.TranNoItem = UkfDetail.TranNoItem.Value.Value;
            if (UkfDetail.WeekNo is not null && UkfDetail.WeekNo.Value.HasValue) u.WeekNo = UkfDetail.WeekNo.Value.Value;

            return u;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="E01Detail"></param>
        ///// <returns></returns>
        //public static KfE1E3Transaction FileToDb(E01Detail E01Detail)
        //{
        //    KfE1E3Transaction d = new KfE1E3Transaction();


        //    if (E01Detail.TransactionNumber.Value.HasValue) d.TransactionNumber = E01Detail.TransactionNumber.Value.Value;
        //    if (E01Detail.TransactionSequence.Value.HasValue) d.TransactionSequence = (short)E01Detail.TransactionSequence.Value.Value;
        //    if (!string.IsNullOrWhiteSpace(E01Detail.TransactionType.Text)) d.TransactionType = E01Detail.TransactionType.Text;
        //    if (E01Detail.TransactionDate.Value.HasValue) d.TransactionDate = E01Detail.TransactionDate.Value.Value;
        //    if (E01Detail.TransactionTime.Value.HasValue) d.TransactionTime = E01Detail.TransactionTime.Value.Value;
        //    if (E01Detail.Period.Value.HasValue) d.Period = E01Detail.Period.Value.Value;
        //    if (E01Detail.SiteCode.Value.HasValue) d.SiteCode = E01Detail.SiteCode.Value.Value;
        //    if (E01Detail.PumpNumber.Value.HasValue) d.PumpNumber = E01Detail.PumpNumber.Value.Value;
        //    if (E01Detail.CardNumber.Value.HasValue) d.CardNumber = E01Detail.CardNumber.Value;
        //    if (E01Detail.CustomerCode.Value.HasValue) d.CustomerCode = E01Detail.CustomerCode.Value.Value;
        //    if (E01Detail.CustomerAC.Value.HasValue) d.CustomerAc = E01Detail.CustomerAC.Value.Value;
        //    if (!string.IsNullOrWhiteSpace(E01Detail.PrimaryRegistration.Value)) d.PrimaryRegistration = E01Detail.PrimaryRegistration.ToString();
        //    if (E01Detail.Mileage.Value.HasValue) d.Mileage = E01Detail.Mileage.Value.Value;
        //    if (E01Detail.FleetNumber.Value.HasValue) d.FleetNumber = E01Detail.FleetNumber.Value.Value;
        //    if (E01Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E01Detail.ProductCode.Value.Value;
        //    if (E01Detail.Quantity.Value.HasValue) d.Quantity = E01Detail.Quantity.Value.Value;
        //    if (E01Detail.QuantitySign is not null && E01Detail.QuantitySign.Value.HasValue) d.Sign = E01Detail.QuantitySign.Text;
        //    if (E01Detail.Cost.Value.HasValue) d.Cost = E01Detail.Cost.Value.Value;
        //    if (!string.IsNullOrWhiteSpace(E01Detail.CostSign.Text)) d.CostSign = E01Detail.CostSign.Text;
        //    if (E01Detail.AccurateMileage.Value.HasValue) d.AccurateMileage = E01Detail.AccurateMileage.Text;
        //    if (!string.IsNullOrWhiteSpace(E01Detail.CardRegistration.Value)) d.CardRegistration = E01Detail.CardRegistration.ToString();
        //    if (!string.IsNullOrWhiteSpace(E01Detail.TransactionRegistration.Value)) d.TransactonRegistration = E01Detail.TransactionRegistration.ToString();

        //    return d;
        //}
    }
}
