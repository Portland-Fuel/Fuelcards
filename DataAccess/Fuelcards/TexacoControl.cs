using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class TexacoControl
{
    public int ControlId { get; set; }

    public DateOnly? ExportDate { get; set; }

    public int? TotalTransactions { get; set; }

    public double? TotalQuantity { get; set; }
}
