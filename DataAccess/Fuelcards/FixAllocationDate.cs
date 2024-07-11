using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FixAllocationDate
{
    public int Id { get; set; }

    public int? TradeId { get; set; }

    public DateOnly? NewAllocationDate { get; set; }

    public bool? Allocated { get; set; }

    public DateOnly? AllocationEnd { get; set; }

    public virtual ICollection<AllocatedVolume> AllocatedVolumes { get; set; } = new List<AllocatedVolume>();

    public virtual FixedPriceContract? Trade { get; set; }
}
