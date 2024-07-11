using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE4SundrySale
{
    public int Id { get; set; }

    public int? TransactionNumber { get; set; }

    public short? TransactionSequence { get; set; }

    public string? TransactionType { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public TimeOnly? TransactionTime { get; set; }

    public int? CustomerCode { get; set; }

    public short? CustomerAc { get; set; }

    public int? Period { get; set; }

    public short? ProductCode { get; set; }

    public double? Quantity { get; set; }

    public string? QuantitySign { get; set; }

    public double? Value { get; set; }

    public string? ValueSign { get; set; }

    public decimal? CardNumber { get; set; }

    public string? VehicleRegistration { get; set; }

    public string? Reference { get; set; }

    public bool? Invoiced { get; set; }

    public int? ControlId { get; set; }
}
