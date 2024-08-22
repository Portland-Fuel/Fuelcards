using FuelcardModels;
using DataAccess.Fuelcards;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE03
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E03Detail"></param>
        /// <returns>KfE1E3Transaction</returns>
        public static KfE1E3Transaction FileToDb(E03Detail E03Detail)
        {
            KfE1E3Transaction d = new KfE1E3Transaction();

            if (E03Detail.TransactionNumber.Value.HasValue) d.TransactionNumber = E03Detail.TransactionNumber.Value.Value;
            if (E03Detail.TransactionSequence.Value.HasValue) d.TransactionSequence = (short)E03Detail.TransactionSequence.Value.Value;
            if (!string.IsNullOrWhiteSpace(E03Detail.TransactionType.Text)) d.TransactionType = E03Detail.TransactionType.Text;
            if (E03Detail.TransactionDate.Value.HasValue) d.TransactionDate = E03Detail.TransactionDate.Value.Value;
            if (E03Detail.TransactionTime.Value.HasValue) d.TransactionTime = E03Detail.TransactionTime.Value.Value;
            if (E03Detail.Period.Value.HasValue) d.Period = E03Detail.Period.Value.Value;
            if (E03Detail.SiteCode.Value.HasValue) d.SiteCode = E03Detail.SiteCode.Value.Value;
            //if (E03Detail.PumpNumber.Value.HasValue) d.PumpNumber = E03Detail.PumpNumber.Value.Value;
            if (E03Detail.CardNumber.Value.HasValue) d.CardNumber = E03Detail.CardNumber.Value;
            if (E03Detail.CustomerCode.Value.HasValue) d.CustomerCode = E03Detail.CustomerCode.Value.Value;
            if (E03Detail.CustomerAC.Value.HasValue) d.CustomerAc = E03Detail.CustomerAC.Value.Value;
            if (!string.IsNullOrWhiteSpace(E03Detail.PrimaryRegistration.Value)) d.PrimaryRegistration = E03Detail.PrimaryRegistration.ToString();
            if (E03Detail.Mileage.Value.HasValue) d.Mileage = E03Detail.Mileage.Value.Value;
            if (E03Detail.FleetNumber.Value.HasValue) d.FleetNumber = E03Detail.FleetNumber.Value.Value;
            if (E03Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E03Detail.ProductCode.Value.Value;
            if (E03Detail.Quantity.Value.HasValue) d.Quantity = E03Detail.Quantity.Value.Value;
            if (E03Detail.QuantitySign.Value.HasValue) d.Sign = E03Detail.QuantitySign.Text;
            if (E03Detail.Value.Value.HasValue) d.Cost = E03Detail.Value.Value.Value;
            if (!string.IsNullOrWhiteSpace(E03Detail.ValueSign.Text)) d.CostSign = E03Detail.ValueSign.Text;
            if (E03Detail.AccurateMileage.Value.HasValue) d.AccurateMileage = E03Detail.AccurateMileage.Text;
            if (!string.IsNullOrWhiteSpace(E03Detail.CardRegistration.Value)) d.CardRegistration = E03Detail.CardRegistration.ToString();
            if (!string.IsNullOrWhiteSpace(E03Detail.TransactionRegistration.Value)) d.TransactonRegistration = E03Detail.TransactionRegistration.ToString();

            return d;
        }
    }
}

