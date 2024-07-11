using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class AllocatedVolume
{
    public int Id { get; set; }

    public int? TradeId { get; set; }

    public double? Volume { get; set; }

    public int? AllocationId { get; set; }

    public virtual FixAllocationDate? Allocation { get; set; }

    public virtual FixedPriceContract? Trade { get; set; }
}
