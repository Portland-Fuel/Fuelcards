using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FgTransaction
{
    public int TransactionId { get; set; }

    public DateOnly? FileProcessDate { get; set; }

    public long? MerchantId { get; set; }

    public string? MerchantName { get; set; }

    public string? Supermarket { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public TimeOnly? TransactionTime { get; set; }

    public int? EftNumber { get; set; }

    public long? CustomerNumber { get; set; }

    public string? PanNumber { get; set; }

    public string? CardName { get; set; }

    public string? RegNo { get; set; }

    public int? Mileage { get; set; }

    public short? ProductNumber { get; set; }

    public string? ProductName { get; set; }

    public short? ProductCode { get; set; }

    public double? ProductQuantity { get; set; }

    public double? GrossAmount { get; set; }

    public string? PurchaseRefund { get; set; }

    public double? NetAmount { get; set; }

    public double? VatAmount { get; set; }

    public double? VatRate { get; set; }

    public int? AuthCode { get; set; }

    public int? PortlandId { get; set; }

    public bool? Invoiced { get; set; }
}
