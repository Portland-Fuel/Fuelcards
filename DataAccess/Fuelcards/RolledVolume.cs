using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class RolledVolume
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public int? TradeReferenceId { get; set; }

    public int? Period { get; set; }

    public double? VolumeRolled { get; set; }

    public DateOnly? DateOfAllocation { get; set; }

    public bool? IsCurrent { get; set; }

    public virtual FixedPriceContract? TradeReference { get; set; }
}
