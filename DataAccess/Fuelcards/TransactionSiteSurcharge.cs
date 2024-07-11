using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class TransactionSiteSurcharge
{
    public DateOnly EffectiveDate { get; set; }

    public int? Network { get; set; }

    public double? Surcharge { get; set; }

    public string? ChargeType { get; set; }
}
