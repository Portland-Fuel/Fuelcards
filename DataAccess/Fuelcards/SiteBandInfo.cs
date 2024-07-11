using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class SiteBandInfo
{
    public int Id { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public int? NetworkId { get; set; }

    public string? Band { get; set; }

    public double? CommercialPrice { get; set; }
}
