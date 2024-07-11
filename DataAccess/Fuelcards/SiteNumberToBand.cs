using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class SiteNumberToBand
{
    public int Id { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public int NetworkId { get; set; }

    public int? SiteNumber { get; set; }

    public string? Brand { get; set; }

    public string? Band { get; set; }

    public double? ShareTheBurdon { get; set; }

    public double? Classification { get; set; }

    public double? Surcharge { get; set; }

    public string? Name { get; set; }

    public bool? Active { get; set; }

    public virtual FcNetwork Network { get; set; } = null!;
}
