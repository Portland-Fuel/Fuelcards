using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE11
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E11Detail"></param>
        /// <returns></returns>
        public static KfE11Product FileToDb(E11Detail E11Detail)
        {
            KfE11Product d = new KfE11Product();


            if (E11Detail.ProductCode.Value.HasValue) d.ProductCode = E11Detail.ProductCode.Value.Value;
            if (!string.IsNullOrWhiteSpace(E11Detail.ProductDescription.Value)) d.ProductDescription = E11Detail.ProductDescription.ToString();

            return d;
        }
    }
}
