using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE21
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E21Detail"></param>
        /// <returns></returns>
        public static KfE21Account FileToDb(E21Detail E21Detail)
        {
            KfE21Account d = new KfE21Account();

            if (E21Detail.CustomerAccountCode.Value.HasValue) d.CustomerAccountCode = E21Detail.CustomerAccountCode.Value.Value;
            if (E21Detail.CustomerAccountSuffix.Value.HasValue) d.CustomerAccountSuffix = (short)E21Detail.CustomerAccountSuffix.Value.Value;
            if (E21Detail.Date.Value.HasValue) d.Date = E21Detail.Date.Value.Value;
            if (E21Detail.Time.Value.HasValue) d.Time = E21Detail.Time.Value.Value;
            if (E21Detail.ActionStatus.Value.HasValue) d.ActionStatus = E21Detail.ActionStatus.Text;
            if (!string.IsNullOrWhiteSpace(E21Detail.Name.Value)) d.Name = E21Detail.Name.ToString();
            if (!string.IsNullOrWhiteSpace(E21Detail.AddressLine1.Value)) d.AddressLine1 = E21Detail.AddressLine1.ToString();
            if (!string.IsNullOrWhiteSpace(E21Detail.AddressLine2.Value)) d.AddressLine2 = E21Detail.AddressLine2.ToString();
            if (!string.IsNullOrWhiteSpace(E21Detail.Town.Value)) d.Town = E21Detail.Town.ToString();
            if (!string.IsNullOrWhiteSpace(E21Detail.County.Value)) d.County = E21Detail.County.ToString();
            if (!string.IsNullOrWhiteSpace(E21Detail.PostCode.Value)) d.Postcode = E21Detail.PostCode.ToString();

            return d;
        }
    }
}
