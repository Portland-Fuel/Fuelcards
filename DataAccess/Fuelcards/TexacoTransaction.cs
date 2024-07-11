using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class TexacoTransaction
{
    public int TransactionId { get; set; }

    public int? ControlId { get; set; }

    public int? PortlandId { get; set; }

    public short? Batch { get; set; }

    public short? Division { get; set; }

    public char? ClientType { get; set; }

    public long? Customer { get; set; }

    public long? Site { get; set; }

    public DateOnly? TranDate { get; set; }

    public TimeOnly? TranTime { get; set; }

    public int? CardNo { get; set; }

    public string? Registration { get; set; }

    public int? Mileage { get; set; }

    public double? Quantity { get; set; }

    public int? ProdNo { get; set; }

    public int? MonthNo { get; set; }

    public int? WeekNo { get; set; }

    public int? TranNoItem { get; set; }

    public double? Price { get; set; }

    public int? IsoNumber { get; set; }

    public bool? Invoiced { get; set; }

    public double? Commission { get; set; }

    public double? InvoicePrice { get; set; }

    public int? InvoiceNumber { get; set; }
}
