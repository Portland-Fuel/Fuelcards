using FuelcardModels;

using DataAccess.Fuelcards;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE04
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E04Detail"></param>
        /// <returns></returns>
        public static KfE4SundrySale FileToDb(E04Detail E04Detail)
        {
            KfE4SundrySale d = new KfE4SundrySale();


            if (E04Detail.TransactionNumber.Value.HasValue) d.TransactionNumber = E04Detail.TransactionNumber.Value.Value;
            if (E04Detail.TransactionSequence.Value.HasValue) d.TransactionSequence = (short)E04Detail.TransactionSequence.Value.Value;
            if (!string.IsNullOrWhiteSpace(E04Detail.TransactionType.Text)) d.TransactionType = E04Detail.TransactionType.Text;
            if (E04Detail.TransactionDate.Value.HasValue) d.TransactionDate = E04Detail.TransactionDate.Value.Value;
            if (E04Detail.TransactionTime.Value.HasValue) d.TransactionTime = E04Detail.TransactionTime.Value.Value;
            if (E04Detail.CustomerCode.Value.HasValue) d.CustomerCode = E04Detail.CustomerCode.Value.Value;
            if (E04Detail.CustomerAC.Value.HasValue) d.CustomerAc = E04Detail.CustomerAC.Value.Value;
            if (E04Detail.Period.Value.HasValue) d.Period = E04Detail.Period.Value.Value;
            if (E04Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E04Detail.ProductCode.Value.Value;
            if (E04Detail.Quantity.Value.HasValue) d.Quantity = E04Detail.Quantity.Value.Value;
            if (E04Detail.QuantitySign.Value.HasValue) d.QuantitySign = E04Detail.QuantitySign.Text;
            if (E04Detail.Value.Value.HasValue) d.Value = E04Detail.Value.Value.Value;
            if (E04Detail.ValueSign.Value.HasValue) d.ValueSign = E04Detail.ValueSign.Text;
            if (E04Detail.CardNumber is not null && E04Detail.CardNumber.Value.HasValue) d.CardNumber = E04Detail.CardNumber.Value;
            if (E04Detail.VehicleRegistration is not null && !string.IsNullOrWhiteSpace(E04Detail.VehicleRegistration.Value)) d.VehicleRegistration = E04Detail.VehicleRegistration.ToString();
            if (E04Detail.Reference is not null && !string.IsNullOrWhiteSpace(E04Detail.Reference.Value)) d.Reference = E04Detail.Reference.ToString();

            return d;
        }

    }
}
