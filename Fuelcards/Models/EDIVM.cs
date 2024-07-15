using DataAccess.Fuelcards;

namespace Fuelcards.Models
{
    public class EDIVM
    {
        public List<string> FileNames { get; set; }
        public List<FcControlVM> EDIs { get; set; }
    }
    public partial class FcControlVM
    {
        public int ControlId { get; set; }

        public short ReportType { get; set; }

        public int? BatchNumber { get; set; }

        public DateOnly? CreationDate { get; set; }

        public TimeOnly? CreationTime { get; set; }

        public int? CustomerCode { get; set; }

        public short? CustomerAc { get; set; }

        public int? RecordCount { get; set; }

        public double? TotalQuantity { get; set; }

        public string QuantitySign { get; set; }

        public double? TotalCost { get; set; }

        public string CostSign { get; set; }

        public bool? Invoiced { get; set; }

        public int? Network { get; set; }
        public DayOfWeek DayOfTheWeek { get; set; }

        internal static FcControlVM Map(FcControl item)
        {
            FcControlVM fcControlVM = new()
            {
                ControlId = item.ControlId,
                ReportType = item.ReportType,
                BatchNumber = item.BatchNumber,
                CreationDate = item.CreationDate,
                CreationTime = item.CreationTime,
                CustomerCode = item.CustomerCode,
                CustomerAc = item.CustomerAc,
                RecordCount = item.RecordCount,
                TotalQuantity = item.TotalQuantity,
                QuantitySign = item.QuantitySign,
                TotalCost = item.TotalCost,
                CostSign = item.CostSign,
                Invoiced = item.Invoiced,
                Network = item.Network
            };
            return fcControlVM;
        }
    }

}
