using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE5Stock
{
    public int Id { get; set; }

    public int? CustomerCode { get; set; }

    public short? CustomerAc { get; set; }

    public short? ProductCode { get; set; }

    public double? OpeningStockBalance { get; set; }

    public string? OpeningBalanceSign { get; set; }

    public double? DrawingQuantity { get; set; }

    public string? DrawingQuantitySign { get; set; }

    public int? NumberOfDrawings { get; set; }

    public double? DeliveryQuantity { get; set; }

    public string? DeliveryQuantitySign { get; set; }

    public int? NumberOfDeliveries { get; set; }

    public double? ClosingStockBalance { get; set; }

    public string? ClosingBalanceSign { get; set; }

    public int? Period { get; set; }

    public int? ControlId { get; set; }
}
