using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE22
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E22Detail"></param>
        /// <returns></returns>
        public static KfE22AccountsStopped FileToDb(E22Detail E22Detail)
        {
            KfE22AccountsStopped d = new KfE22AccountsStopped();


            if (E22Detail.CustomerAccountCode.Value.HasValue) d.CustomerAccountCode = E22Detail.CustomerAccountCode.Value.Value;
            if (E22Detail.CustomerAccountSuffix.Value.HasValue) d.CustomerAccountSuffix = (short)E22Detail.CustomerAccountSuffix.Value.Value;
            if (E22Detail.Date.Value.HasValue) d.Date = E22Detail.Date.Value.Value;
            if (E22Detail.Time.Value.HasValue) d.Time = E22Detail.Time.Value.Value;
            if (E22Detail.StopStatusCode.Value.HasValue) d.StopStatusCode = E22Detail.StopStatusCode.Text;
            if (!string.IsNullOrWhiteSpace(E22Detail.PersonWhoRequestedStop.Value)) d.PersonRequestingStop = E22Detail.PersonWhoRequestedStop.ToString();
            if (E22Detail.StopReferenceNumber.Value.HasValue) d.StopReferenceNumber = E22Detail.StopReferenceNumber.Value.Value;

            return d;
        }
    }
}
