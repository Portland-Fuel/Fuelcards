using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class UkfTransaction
{
    public int TransactionId { get; set; }

    public int ControlId { get; set; }

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

    public int? Quantity { get; set; }

    public short? ProdNo { get; set; }

    public short? MonthNo { get; set; }

    public short? WeekNo { get; set; }

    public int? TranNoItem { get; set; }

    public int? Price { get; set; }

    public decimal? PanNumber { get; set; }

    public bool? Invoiced { get; set; }

    public string? ReceiptNo { get; set; }

    public double? Commission { get; set; }

    public double? InvoicePrice { get; set; }

    public int? InvoiceNumber { get; set; }

    public double? UnitPrice { get; set; }
}
