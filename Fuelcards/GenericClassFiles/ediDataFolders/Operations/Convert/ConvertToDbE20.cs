using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE20
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E20Detail"></param>
        /// <returns></returns>
        public static KfE20StoppedCard FileToDb(E20Detail E20Detail)
        {
            KfE20StoppedCard d = new KfE20StoppedCard();


            if (E20Detail.CustomerAccountCode.Value.HasValue) d.CustomerAccountCode = E20Detail.CustomerAccountCode.Value.Value;
            if (E20Detail.CustomerAccountSuffix.Value.HasValue) d.CustomerAccountSuffix = E20Detail.CustomerAccountSuffix.Value.Value;
            if (E20Detail.PAN.Value.HasValue) d.PanNumber = E20Detail.PAN.Value;
            if (E20Detail.Date.Value.HasValue) d.Date = E20Detail.Date.Value.Value;
            if (E20Detail.Time.Value.HasValue) d.Time = E20Detail.Time.Value.Value;
            if (E20Detail.StopCode.Value.HasValue) d.StopCode = E20Detail.StopCode.Text;

            return d;
        }
    }
}
