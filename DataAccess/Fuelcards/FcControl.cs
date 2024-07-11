using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcControl
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

    public string? QuantitySign { get; set; }

    public double? TotalCost { get; set; }

    public string? CostSign { get; set; }

    public bool? Invoiced { get; set; }

    public int? Network { get; set; }
}
