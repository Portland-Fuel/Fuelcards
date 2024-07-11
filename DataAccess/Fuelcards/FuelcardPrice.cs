using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FuelcardPrice
{
    public int Id { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public double? FixedPrice { get; set; }

    public int? FixedVolume { get; set; }

    public int? FuelcardAccountNumber { get; set; }

    public int? Network { get; set; }
}
