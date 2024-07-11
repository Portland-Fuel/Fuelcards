using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE1E3Transaction
{
    public int TransactionId { get; set; }

    public int ControlId { get; set; }

    public int? PortlandId { get; set; }

    public bool? ReportType { get; set; }

    public int? TransactionNumber { get; set; }

    public short? TransactionSequence { get; set; }

    public string? TransactionType { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public TimeOnly? TransactionTime { get; set; }

    public int? Period { get; set; }

    public int? SiteCode { get; set; }

    public short? PumpNumber { get; set; }

    public decimal? CardNumber { get; set; }

    public int? CustomerCode { get; set; }

    public short? CustomerAc { get; set; }

    public string? PrimaryRegistration { get; set; }

    public int? Mileage { get; set; }

    public int? FleetNumber { get; set; }

    public short? ProductCode { get; set; }

    public double? Quantity { get; set; }

    public string? Sign { get; set; }

    public double? Cost { get; set; }

    public string? CostSign { get; set; }

    public string? AccurateMileage { get; set; }

    public string? CardRegistration { get; set; }

    public string? TransactonRegistration { get; set; }

    public bool? Invoiced { get; set; }

    public double? Commission { get; set; }

    public double? InvoicePrice { get; set; }

    public int? InvoiceNumber { get; set; }
}
