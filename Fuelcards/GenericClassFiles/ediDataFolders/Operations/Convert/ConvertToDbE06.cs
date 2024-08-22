using FuelcardModels;

using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE06
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E06Detail"></param>
        /// <returns></returns>
        public static KfE6Transfer FileToDb(E06Detail E06Detail)
        {
            KfE6Transfer d = new KfE6Transfer();

            if (E06Detail.TransactionNumber.Value.HasValue) d.TransactionNumber = E06Detail.TransactionNumber.Value.Value;
            if (E06Detail.TransactionSequence.Value.HasValue) d.TransactionSequence = (short)E06Detail.TransactionSequence.Value.Value;
            if (!string.IsNullOrWhiteSpace(E06Detail.Narrative.Value)) d.Narrative = E06Detail.Narrative.ToString();

            return d;
        }
    }
}
