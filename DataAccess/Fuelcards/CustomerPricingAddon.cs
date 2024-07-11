using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class CustomerPricingAddon
{
    public int Id { get; set; }

    public DateOnly? EffectiveDate { get; set; }

    public int? PortlandId { get; set; }

    public int? Network { get; set; }

    public double? Addon { get; set; }
}
