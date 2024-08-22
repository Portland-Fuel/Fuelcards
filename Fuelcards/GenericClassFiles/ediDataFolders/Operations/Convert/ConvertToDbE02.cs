using FuelcardModels;
using DataAccess.Fuelcards;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbE02
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E02Detail"></param>
        /// <returns>KfE1E3Transaction</returns>
        public static KfE2Delivery FileToDb(E02Detail E02Detail)
        {
            KfE2Delivery d = new KfE2Delivery();

            if (E02Detail.TransactionNumber.Value.HasValue) d.TransactionNumber = E02Detail.TransactionNumber.Value.Value;
            if (E02Detail.TransactionSequence.Value.HasValue) d.TransactionSequence = (short)E02Detail.TransactionSequence.Value.Value;
            if (!string.IsNullOrWhiteSpace(E02Detail.TransactionType.Text)) d.TransactionType = E02Detail.TransactionType.Text;
            if (E02Detail.TransactionDate.Value.HasValue) d.TransactionDate = E02Detail.TransactionDate.Value.Value;
            if (E02Detail.TransactionTime.Value.HasValue) d.TransactionTime = E02Detail.TransactionTime.Value.Value;
            if (E02Detail.Period.Value.HasValue) d.Period = E02Detail.Period.Value.Value;
            if (E02Detail.SiteCode.Value.HasValue) d.SiteCode = E02Detail.SiteCode.Value.Value;
            if (E02Detail.CustomerCode.Value.HasValue) d.CustomerCode = E02Detail.CustomerCode.Value.Value;
            if (E02Detail.CustomerAC.Value.HasValue) d.CustomerAc = E02Detail.CustomerAC.Value.Value;
            if (!string.IsNullOrWhiteSpace(E02Detail.SupplierName.Value)) d.SupplierName = E02Detail.SupplierName.ToString();
            if (E02Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E02Detail.ProductCode.Value.Value;
            if (E02Detail.ProductCode.Value.HasValue) d.ProductCode = (short)E02Detail.ProductCode.Value.Value;
            if (E02Detail.Quantity.Value.HasValue) d.Quantity = E02Detail.Quantity.Value.Value;
            if (E02Detail.QuantitySign.Value.HasValue) d.QuantitySign = E02Detail.QuantitySign.Text;
            if (!string.IsNullOrWhiteSpace(E02Detail.CustOwnOrderNo.Value)) d.CustomerOwnOrderNo = E02Detail.CustOwnOrderNo.ToString();
            if (!string.IsNullOrWhiteSpace(E02Detail.CustomerOrderNo.Value)) d.CustomerOrderNo = E02Detail.CustomerOrderNo.ToString();
            if (!string.IsNullOrWhiteSpace(E02Detail.HandlingCharge.Text)) d.HandlingCharge = E02Detail.HandlingCharge.Text;
            if (!string.IsNullOrWhiteSpace(E02Detail.DeliveryNoteNo.Value)) d.DeliveryNoteNo = E02Detail.DeliveryNoteNo.ToString();

            return d;
        }
    }
}
