using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE2Delivery
{
    public int Id { get; set; }

    public int? TransactionNumber { get; set; }

    public short? TransactionSequence { get; set; }

    public string? TransactionType { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public TimeOnly? TransactionTime { get; set; }

    public int? Period { get; set; }

    public int? SiteCode { get; set; }

    public int? CustomerCode { get; set; }

    public short? CustomerAc { get; set; }

    public string? SupplierName { get; set; }

    public short? ProductCode { get; set; }

    public double? Quantity { get; set; }

    public string? QuantitySign { get; set; }

    public string? CustomerOwnOrderNo { get; set; }

    public string? CustomerOrderNo { get; set; }

    public string? HandlingCharge { get; set; }

    public string? DeliveryNoteNo { get; set; }

    public int? ControlId { get; set; }
}
