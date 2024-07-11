using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class AllocatedVolumeTemp
{
    public int Id { get; set; }

    public int? TradeId { get; set; }

    public double? Volume { get; set; }

    public int? AllocationId { get; set; }
}
