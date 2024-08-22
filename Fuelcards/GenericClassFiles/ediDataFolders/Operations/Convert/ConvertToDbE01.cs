using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuelcardModels;
using DataAccess.Repositorys.IRepositorys;
using DataAccess.Fuelcards;
namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE01
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E01Detail"></param>
        /// <returns></returns>
        public static KfE1E3Transaction FileToDb(E01Detail E01Detail)
        {
            KfE1E3Transaction d = new KfE1E3Transaction();


            if (E01Detail.TransactionNumber.Value.HasValue) d.TransactionNumber = E01Detail.TransactionNumber.Value.Value;
            if (E01Detail.TransactionSequence.Value.HasValue) d.TransactionSequence = (short)E01Detail.TransactionSequence.Value.Value;
            if(!string.IsNullOrWhiteSpace(E01Detail.TransactionType.Text)) d.TransactionType = E01Detail.TransactionType.Text;
            if(E01Detail.TransactionDate.Value.HasValue) d.TransactionDate = E01Detail.TransactionDate.Value.Value;
            if (E01Detail.TransactionTime.Value.HasValue) d.TransactionTime = E01Detail.TransactionTime.Value.Value;
            if(E01Detail.Period.Value.HasValue) d.Period = E01Detail.Period.Value.Value;
            if(E01Detail.SiteCode.Value.HasValue) d.SiteCode = E01Detail.SiteCode.Value.Value;
            if(E01Detail.PumpNumber.Value.HasValue) d.PumpNumber = E01Detail.PumpNumber.Value.Value;
            if (E01Detail.CardNumber.Value.HasValue) d.CardNumber =  E01Detail.CardNumber.Value;
            if(E01Detail.CustomerCode.Value.HasValue) d.CustomerCode = E01Detail.CustomerCode.Value.Value;
            if (E01Detail.CustomerAC.Value.HasValue) d.CustomerAc = E01Detail.CustomerAC.Value.Value;
            if (!string.IsNullOrWhiteSpace(E01Detail.PrimaryRegistration.Value)) d.PrimaryRegistration = E01Detail.PrimaryRegistration.ToString();
            if (E01Detail.Mileage.Value.HasValue) d.Mileage = E01Detail.Mileage.Value.Value;
            if (E01Detail.FleetNumber.Value.HasValue) d.FleetNumber = E01Detail.FleetNumber.Value.Value;
            if (E01Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E01Detail.ProductCode.Value.Value;
            if (E01Detail.Quantity.Value.HasValue) d.Quantity = E01Detail.Quantity.Value.Value;
            if (E01Detail.QuantitySign is not null && E01Detail.QuantitySign.Value.HasValue) d.Sign = E01Detail.QuantitySign.Text;
            if (E01Detail.Cost.Value.HasValue) d.Cost = E01Detail.Cost.Value.Value;
            if (!string.IsNullOrWhiteSpace(E01Detail.CostSign.Text)) d.CostSign = E01Detail.CostSign.Text;
            if (E01Detail.AccurateMileage.Value.HasValue) d.AccurateMileage = E01Detail.AccurateMileage.Text;
            if (!string.IsNullOrWhiteSpace(E01Detail.CardRegistration.Value)) d.CardRegistration = E01Detail.CardRegistration.ToString();
            if (!string.IsNullOrWhiteSpace(E01Detail.TransactionRegistration.Value)) d.TransactonRegistration = E01Detail.TransactionRegistration.ToString();

            return d;
        }

    }
}
