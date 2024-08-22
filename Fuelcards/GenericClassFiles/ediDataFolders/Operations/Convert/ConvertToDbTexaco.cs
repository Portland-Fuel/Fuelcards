using FuelcardModels;
using DataAccess.Fuelcards;


namespace FuelCardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertToDbTexaco
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TexDetail"></param>
        /// <returns></returns>
        public static TexacoTransaction FileToDb(TexacoDetail TexDetail)
        {
            TexacoTransaction t = new TexacoTransaction();

            if (TexDetail.Batch is not null && TexDetail.Batch.Value.HasValue) t.Batch = (short)TexDetail.Batch.Value.Value;
            if (TexDetail.Division is not null && TexDetail.Division.Value.HasValue) t.Division = (short)TexDetail.Division.Value.Value;
            if (TexDetail.ClientType is not null && TexDetail.ClientType.Value.HasValue) t.ClientType = TexDetail.ClientType.Value.Value;
            if (TexDetail.Customer is not null && TexDetail.Customer.Value.HasValue) t.Customer = TexDetail.Customer.Value.Value;
            if (TexDetail.TranDate is not null && TexDetail.TranDate.Value.HasValue) t.TranDate = TexDetail.TranDate.Value.Value;
            if (TexDetail.TranTime is not null && TexDetail.TranTime.Value.HasValue) t.TranTime = TexDetail.TranTime.Value.Value;
            if (TexDetail.Site is not null && TexDetail.Site.Value.HasValue) t.Site = TexDetail.Site.Value.Value;
            if (TexDetail.CardNo is not null && TexDetail.CardNo.Value.HasValue) t.CardNo = (int)TexDetail.CardNo.Value.Value;
            if (TexDetail.Registration is not null && !string.IsNullOrWhiteSpace(TexDetail.Registration.Value)) t.Registration = TexDetail.Registration.ToString();
            if (TexDetail.Mileage is not null && TexDetail.Mileage.Value.HasValue) t.Mileage = TexDetail.Mileage.Value.Value;
            if (TexDetail.Quantity is not null && TexDetail.Quantity.Value.HasValue) t.Quantity = TexDetail.Quantity.Value.Value;
            if (TexDetail.ProdNo is not null && TexDetail.ProdNo.Value.HasValue) t.ProdNo = TexDetail.ProdNo.Value.Value;
            if (TexDetail.MonthNo is not null && TexDetail.MonthNo.Value.HasValue) t.MonthNo = TexDetail.MonthNo.Value.Value;
            if (TexDetail.WeekNo is not null && TexDetail.WeekNo.Value.HasValue) t.WeekNo = TexDetail.WeekNo.Value.Value;
            if (TexDetail.TranNoItem is not null && TexDetail.TranNoItem.Value.HasValue) t.TranNoItem = TexDetail.TranNoItem.Value.Value;
            if (TexDetail.Price is not null && TexDetail.Price.Value.HasValue) t.Price = TexDetail.Price.Value.Value;
            if (TexDetail.IsoNumber is not null && TexDetail.IsoNumber.Value.HasValue) t.IsoNumber = TexDetail.IsoNumber.Value.Value;

            return t;
        }
    }
}
