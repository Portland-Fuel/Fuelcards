using FuelcardModels;

using DataAccess.Fuelcards;


namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE05
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E05Detail"></param>
        /// <returns></returns>
        public static KfE5Stock FileToDb(E05Detail E05Detail)
        {
            KfE5Stock d = new KfE5Stock();


            if (E05Detail.CustomerCode.Value.HasValue) d.CustomerCode = E05Detail.CustomerCode.Value.Value;
            if (E05Detail.CustomerAC.Value.HasValue) d.CustomerAc = E05Detail.CustomerAC.Value.Value;
            if (E05Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E05Detail.ProductCode.Value.Value;
            if (E05Detail.OpeningStockBalance.Value.HasValue) d.OpeningStockBalance = E05Detail.OpeningStockBalance.Value.Value;
            if (E05Detail.OpeningBalanceSign.Value.HasValue) d.OpeningBalanceSign = E05Detail.OpeningBalanceSign.Text;
            if (E05Detail.DrawingQuantity.Value.HasValue) d.DrawingQuantity = E05Detail.DrawingQuantity.Value.Value;
            if (E05Detail.DrawingQuantitySign.Value.HasValue) d.DrawingQuantitySign = E05Detail.DrawingQuantitySign.Text;
            if (E05Detail.NumberOfDrawings.Value.HasValue) d.NumberOfDrawings = E05Detail.NumberOfDrawings.Value.Value;
            if (E05Detail.DeliveryQuantity.Value.HasValue) d.DeliveryQuantity = E05Detail.DeliveryQuantity.Value.Value;
            if (E05Detail.DeliveryQuantitySign.Value.HasValue) d.DeliveryQuantitySign = E05Detail.DeliveryQuantitySign.Text;
            if (E05Detail.NumberOfDeliveries.Value.HasValue) d.NumberOfDeliveries = E05Detail.NumberOfDeliveries.Value.Value;
            if (E05Detail.ClosingStockBalance.Value.HasValue) d.ClosingStockBalance = E05Detail.ClosingStockBalance.Value.Value;
            if (E05Detail.ClosingBalanceSign.Value.HasValue) d.ClosingBalanceSign = E05Detail.ClosingBalanceSign.Text;
            if (E05Detail.Period.Value.HasValue) d.Period = E05Detail.Period.Value.Value;

            return d;
        }
    }
}
