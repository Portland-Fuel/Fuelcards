using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FixFrequency
{
    public int FrequencyId { get; set; }

    public string? FrequencyPeriod { get; set; }

    public int? NoDays { get; set; }

    public int? PeriodsPerYear { get; set; }

    public virtual ICollection<FixedPriceContract> FixedPriceContracts { get; set; } = new List<FixedPriceContract>();
}
