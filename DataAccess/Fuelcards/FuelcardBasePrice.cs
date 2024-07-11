using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FuelcardBasePrice
{
    public DateOnly EffectiveFrom { get; set; }

    public DateOnly EffectiveTo { get; set; }

    public double BasePrice { get; set; }
}
